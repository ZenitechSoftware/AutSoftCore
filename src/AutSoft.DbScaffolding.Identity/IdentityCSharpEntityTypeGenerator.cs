using AutSoft.DbScaffolding.Identity.Extensions;

using EntityFrameworkCore.Scaffolding.Handlebars;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AutSoft.DbScaffolding.Identity;

public class IdentityCSharpEntityTypeGenerator : CSharpEntityTypeGenerator
{
    private readonly IOptions<IdentityScaffoldingOptions> _identityOptions;

    public IdentityCSharpEntityTypeGenerator(
        IAnnotationCodeGenerator annotationCodeGenerator,
        ICSharpHelper cSharpHelper,
        IEntityTypeTemplateService entityTypeTemplateService,
        IEntityTypeTransformationService entityTypeTransformationService,
        IOptions<HandlebarsScaffoldingOptions> options,
        IOptions<DbScaffoldingOptions> dbScaffoldingOptions,
        IOptions<IdentityScaffoldingOptions> identityOptions)
        : base(annotationCodeGenerator, cSharpHelper, entityTypeTemplateService, entityTypeTransformationService, options, dbScaffoldingOptions)
    {
        _identityOptions = identityOptions;
    }

    protected override void GenerateClass(IEntityType entityType)
    {
        Check.NotNull(entityType, nameof(entityType));

        if (UseDataAnnotations)
        {
            GenerateEntityTypeDataAnnotations(entityType);
        }

        var identityKeyType = entityType.Model.FindIdentityKeyType(_identityOptions.Value.UserTableName);
        var lookup = _identityOptions.Value.CreateIdentityTableNameLookup(identityKeyType);

        var transformedEntityName = EntityTypeTransformationService.TransformTypeEntityName(entityType.Name);
        var isIdentityTable = lookup.ContainsKey(entityType.Name);
        var identityBaseClass = isIdentityTable ? CSharpHelper.Reference(lookup[entityType.Name]) : string.Empty;

        var implementsList = new List<string>();

        if (isIdentityTable)
            implementsList.Add(identityBaseClass);
        if (entityType.IsDeletable(_identityOptions.Value.DbScaffoldingOptions))
            implementsList.Add(_identityOptions.Value.DbScaffoldingOptions.InterfaceProperties.SoftDeletableInterfaceName);
        if (entityType.IsAuditable(_identityOptions.Value.DbScaffoldingOptions))
            implementsList.Add(_identityOptions.Value.DbScaffoldingOptions.InterfaceProperties.AuditableInterfaceName);

        var implements = implementsList.Count > 0 ? $": {string.Join(", ", implementsList)}" : string.Empty;

        if (_identityOptions?.Value.DbScaffoldingOptions.HandlebarsScaffoldingOptions.GenerateComments == true)
            TemplateData.Add("comment", GenerateComment(entityType.GetComment(), 1));
        TemplateData.Add("class", transformedEntityName);
        TemplateData.Add("is-identity-table", isIdentityTable);
        TemplateData.Add("identity-base-class", identityBaseClass);

        TemplateData.Add("implementations", implements);

        GenerateConstructor(entityType);
        GenerateProperties(entityType);
        GenerateNavigationProperties(entityType);
        GenerateSkipNavigationProperties(entityType);
    }

    protected override void GenerateProperties(IEntityType entityType)
    {
        var identityKeyType = entityType.Model.FindIdentityKeyType(_identityOptions.Value.UserTableName);
        var lookup = _identityOptions.Value.CreateIdentityTableNameLookup(identityKeyType);

        var propertyInfos = entityType.GetProperties();

        if (lookup.ContainsKey(entityType.Name))
        {
            var identityDefinesProperties = lookup[entityType.Name].GetProperties().Select(p => p.Name);
            propertyInfos = propertyInfos.Where(p => !identityDefinesProperties.Contains(p.Name));
            var properties = CreateProperties(entityType, propertyInfos);
            var transformedProperties = EntityTypeTransformationService.TransformProperties(entityType, properties);
            TemplateData.Add("properties", transformedProperties);
        }
        else
        {
            base.GenerateProperties(entityType);
        }
    }
}
