using AutSoft.DbScaffolding.Configuration;
using AutSoft.DbScaffolding.Helpers;

using EntityFrameworkCore.Scaffolding.Handlebars;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AutSoft.DbScaffolding.Generators;

internal class CSharpEntityTypeGenerator : HbsCSharpEntityTypeGenerator
{
    private readonly IOptions<DbScaffoldingOptions> _dbScaffoldingOptions;

    public CSharpEntityTypeGenerator(
        IAnnotationCodeGenerator annotationCodeGenerator,
        ICSharpHelper cSharpHelper,
        IEntityTypeTemplateService entityTypeTemplateService,
        IEntityTypeTransformationService entityTypeTransformationService,
        IOptions<HandlebarsScaffoldingOptions> options,
        IOptions<DbScaffoldingOptions> dbScaffoldingOptions)
        : base(annotationCodeGenerator, cSharpHelper, entityTypeTemplateService, entityTypeTransformationService, options)
    {
        _dbScaffoldingOptions = dbScaffoldingOptions;
    }

    protected internal List<Dictionary<string, object>> CreateProperties(IEntityType entityType, IEnumerable<IProperty> propertyInfos)
    {
        var properties = new List<Dictionary<string, object>>();

        foreach (var property in propertyInfos.OrderBy(p => p.GetColumnOrder() ?? -1))
        {
            PropertyAnnotationsData = new List<Dictionary<string, object>>();

            if (UseDataAnnotations)
                GeneratePropertyDataAnnotations(property);

            var enumType = _dbScaffoldingOptions
                .Value
                .ColumnToEnumDictionary
                .GetValueOrDefault(new DbColumn(entityType.GetSchemaName(), entityType.Name, property.Name));

            if (enumType != null && Nullable.GetUnderlyingType(property.ClrType) != null)
                enumType = typeof(Nullable<>).MakeGenericType(enumType);

            var spatialType = _dbScaffoldingOptions
                .Value
                .ColumnToSpatialTypeDictionary
                .GetValueOrDefault(new DbColumn(entityType.GetSchemaName(), entityType.Name, property.Name));

            if (spatialType != null && Nullable.GetUnderlyingType(property.ClrType) != null)
                spatialType = typeof(Nullable<>).MakeGenericType(spatialType);

            var propertyType = CSharpHelper.Reference(enumType ?? spatialType ?? property.ClrType);
            if (UseNullableReferenceTypes
                && property.IsNullable
                && !propertyType.EndsWith("?", StringComparison.OrdinalIgnoreCase))
            {
                propertyType += "?";
            }

            var propertyIsNullable = property.ClrType.IsValueType || (UseNullableReferenceTypes && property.IsNullable);

            properties.Add(new Dictionary<string, object>
            {
                { "property-type", propertyType },
                { "property-name", property.Name },
                { "property-annotations",  PropertyAnnotationsData },
                { "property-comment", _dbScaffoldingOptions?.Value?.HandlebarsScaffoldingOptions.GenerateComments == true ? GenerateComment(property.GetComment(), 2) : null },
                { "property-isnullable", propertyIsNullable },
                { "nullable-reference-types", UseNullableReferenceTypes },
            });
        }

        return properties;
    }

    protected override void GenerateImports(IEntityType entityType, string defaultNamespace, bool enableSchemaFolders)
    {
        base.GenerateImports(entityType, defaultNamespace, enableSchemaFolders);

        var imports = (List<Dictionary<string, object>>)TemplateData["imports"];

        var enumImports = GetEnumImports(entityType);
        var interfaceImports = GetInterfaceImports(entityType);
        var allImports = new HashSet<string>(enumImports.Concat(interfaceImports));

        imports.AddRange(allImports.Select(import => new Dictionary<string, object> { ["import"] = import }));
    }

    protected override void GenerateProperties(IEntityType entityType)
    {
        Check.NotNull(entityType, nameof(entityType));

        var propertyInfos = entityType.GetProperties();
        var properties = CreateProperties(entityType, propertyInfos);
        var transformedProperties = EntityTypeTransformationService.TransformProperties(entityType, properties);

        TemplateData.Add("properties", transformedProperties);
    }

    protected override void GenerateClass(IEntityType entityType)
    {
        Check.NotNull(entityType, nameof(entityType));

        if (UseDataAnnotations)
            GenerateEntityTypeDataAnnotations(entityType);

        var transformedEntityName = EntityTypeTransformationService.TransformTypeEntityName(entityType.Name);

        var interfaces = new HashSet<string>();

        if (entityType.IsDeletable(_dbScaffoldingOptions.Value))
            interfaces.Add(_dbScaffoldingOptions.Value.InterfaceProperties.SoftDeletableInterfaceName);
        if (entityType.IsAuditable(_dbScaffoldingOptions.Value))
            interfaces.Add(_dbScaffoldingOptions.Value.InterfaceProperties.AuditableInterfaceName);

        var implements = interfaces.Count > 0
            ? $": {string.Join(", ", interfaces)}"
            : string.Empty;

        if (_dbScaffoldingOptions?.Value.HandlebarsScaffoldingOptions.GenerateComments == true)
            TemplateData.Add("comment", GenerateComment(entityType.GetComment(), 1));
        TemplateData.Add("class", transformedEntityName);
        TemplateData.Add("implementations", implements);

        GenerateConstructor(entityType);
        GenerateProperties(entityType);
        GenerateNavigationProperties(entityType);
        GenerateSkipNavigationProperties(entityType);
    }

    protected string GenerateComment(string comment, int indents)
    {
        var sb = new IndentedStringBuilder();
        if (!string.IsNullOrWhiteSpace(comment))
        {
            for (var i = 0; i < indents; i++)
                sb.IncrementIndent();
            foreach (var line in comment.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
            {
                sb.AppendLine($"/// {System.Security.SecurityElement.Escape(line)}");
            }

            for (var i = 0; i < indents; i++)
                sb.DecrementIndent();
        }

        return sb.ToString().Trim(Environment.NewLine.ToCharArray());
    }

    private IEnumerable<string> GetEnumImports(IEntityType entityType)
    {
        var propertyInfos = entityType.GetProperties();
        var imports = new HashSet<string>();

        foreach (var property in propertyInfos.OrderBy(p => p.GetColumnOrder() ?? -1))
        {
            if (_dbScaffoldingOptions
                .Value
                .ColumnToEnumDictionary
                .TryGetValue(new DbColumn(entityType.GetSchemaName(), entityType.Name, property.Name), out var enumType))
            {
                imports.Add(enumType.Namespace);
            }
        }

        return imports;
    }

    private IEnumerable<string> GetInterfaceImports(IEntityType entityType)
    {
        var imports = new HashSet<string>();

        if (entityType.IsDeletable(_dbScaffoldingOptions.Value)
            || entityType.IsAuditable(_dbScaffoldingOptions.Value))
        {
            imports.Add(_dbScaffoldingOptions.Value.InterfaceProperties.InterfaceNameSpace);
        }

        return imports;
    }
}
