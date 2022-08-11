using AutSoft.DbScaffolding.EntityAbstractions.Services;
using EntityFrameworkCore.Scaffolding.Handlebars;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutSoft.DbScaffolding.EntityAbstractions
{
    public class InterfaceGenerator : IInterfaceGenerator
    {
        private readonly DbScaffoldingOptions _options;
        private readonly IInterfaceTemplateService _interfaceTemplateService;
        private readonly IEntityTypeTransformationService _transformationService;

        public InterfaceGenerator(IOptions<DbScaffoldingOptions> dbScaffoldOptions, IInterfaceTemplateService interfaceTemplateService, IEntityTypeTransformationService transformationService)
        {
            _options = dbScaffoldOptions.Value;
            _interfaceTemplateService = interfaceTemplateService;
            _transformationService = transformationService;
        }

        public string GenerateSoftDeletableInterface(IEntityType entityType, bool useNullableReferenceTypes)
        {
            var templateData = new Dictionary<string, object>();

            var properties = GenerateProperties(entityType, new Dictionary<string, string> { { _options.InterfaceProperties.IsDeleted, "bool" } }, useNullableReferenceTypes);

            templateData.Add("properties", properties);
            templateData.Add("namespace", _options.InterfaceProperties.InterfaceNameSpace);
            templateData.Add("interface", _options.InterfaceProperties.SoftDeletableInterfaceName);

            return _interfaceTemplateService.GenerateInterface(templateData);
        }

        public string GenerateAuditableInterface(IEntityType entityType, bool useNullableReferenceTypes)
        {
            var templateData = new Dictionary<string, object>();

            var properties = GenerateProperties(entityType, new Dictionary<string, string>
            {
                { _options.InterfaceProperties.LastModAt, nameof(DateTime) },
                { _options.InterfaceProperties.LastModBy, "string" },
                { _options.InterfaceProperties.CreatedAt, nameof(DateTime) },
                { _options.InterfaceProperties.CreatedBy, "string" }
            }, useNullableReferenceTypes);

            templateData.Add("properties", properties);
            templateData.Add("namespace", _options.InterfaceProperties.InterfaceNameSpace);
            templateData.Add("interface", _options.InterfaceProperties.AuditableInterfaceName);

            return _interfaceTemplateService.GenerateInterface(templateData);
        }

        private List<Dictionary<string, object>> GenerateProperties(IEntityType entityType, Dictionary<string, string> properties, bool useNullableReferenceTypes)
        {
            var preTransformedProperties = properties.Select(p =>
                new Dictionary<string, object>
                {
                    { "property-type", p.Value },
                    { "property-name", p.Key },
                    { "property-annotations",  default },
                    { "property-comment", default },
                    { "property-isnullable", false },
                    { "nullable-reference-types", useNullableReferenceTypes }
                }).ToList();

            return _transformationService.TransformProperties(entityType, preTransformedProperties);
        }
    }
}
