using AutSoft.DbScaffolding.Extensions;

using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutSoft.DbScaffolding
{
    public class CSharpDbContextGenerator : HbsCSharpDbContextGenerator
    {
        private readonly IOptions<HandlebarsScaffoldingOptions> _options;
        private readonly IOptions<DbScaffoldingOptions> _dbScaffoldOptions;

        public CSharpDbContextGenerator(
            IProviderConfigurationCodeGenerator providerConfigurationCodeGenerator,
            IAnnotationCodeGenerator annotationCodeGenerator,
            IDbContextTemplateService dbContextTemplateService,
            IEntityTypeTransformationService entityTypeTransformationService,
            ICSharpHelper cSharpHelper,
            IOptions<HandlebarsScaffoldingOptions> options,
            IOptions<DbScaffoldingOptions> dbScaffoldOptions)
            : base(
                providerConfigurationCodeGenerator,
                annotationCodeGenerator,
                dbContextTemplateService,
                entityTypeTransformationService,
                cSharpHelper,
                options)
        {
            _options = options;
            _dbScaffoldOptions = dbScaffoldOptions;
        }

        protected override void GenerateClass(
            IModel model,
            string contextName,
            string connectionString,
            bool suppressConnectionStringWarning,
            bool suppressOnConfiguring)
        {
            base.GenerateClass(model, contextName, connectionString, suppressConnectionStringWarning, suppressOnConfiguring);

            var entities = model.GetScaffoldEntityTypes(_options.Value);
            CheckColumnExistence(entities);
        }

        protected override void GenerateOnConfiguring(string connectionString, bool suppressConnectionStringWarning)
        {
            // –no-onConfiguring flag is megoldaná ugyanezt már ef core 5.0-tól
        }

        private void CheckColumnExistence(IEnumerable<IEntityType> entities)
        {
            var dictionaryKeys = _dbScaffoldOptions.Value.ColumnToEnumDictionary.Keys.Concat(_dbScaffoldOptions.Value.ColumnToSpatialTypeDictionary.Keys);

            foreach (var dbColumn in dictionaryKeys)
            {
                if (!entities
                    .Any(e => e.GetSchemaName() == dbColumn.SchemaName 
                        && e.Name == dbColumn.TableName 
                        && e.GetProperties().Any(p => p.Name == dbColumn.ColumnName)))
                {
                    throw new ArgumentException($"Column ([{dbColumn.SchemaName}].[{dbColumn.TableName}].[{dbColumn.ColumnName}]) not found in database");
                }
            }
        }
    }
}
