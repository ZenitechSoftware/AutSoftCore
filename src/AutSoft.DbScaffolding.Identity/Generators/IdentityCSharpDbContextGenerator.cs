using AutSoft.DbScaffolding.Configuration;
using AutSoft.DbScaffolding.Generators;
using AutSoft.DbScaffolding.Identity.Configuration;
using AutSoft.DbScaffolding.Identity.Extensions;

using EntityFrameworkCore.Scaffolding.Handlebars;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Linq;

namespace AutSoft.DbScaffolding.Identity.Generators;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "We do some low level stuff, so we take the risks.")]
internal class IdentityCSharpDbContextGenerator : CSharpDbContextGenerator
{
    private readonly IOptions<HandlebarsScaffoldingOptions> _options;
    private readonly IOptions<IdentityScaffoldingOptions> _identityOptions;

    public IdentityCSharpDbContextGenerator(
        IProviderConfigurationCodeGenerator providerConfigurationCodeGenerator,
        IAnnotationCodeGenerator annotationCodeGenerator,
        IDbContextTemplateService dbContextTemplateService,
        IEntityTypeTransformationService entityTypeTransformationService,
        ICSharpHelper cSharpHelper,
        IOptions<HandlebarsScaffoldingOptions> options,
        IOptions<IdentityScaffoldingOptions> identityOptions,
        IOptions<DbScaffoldingOptions> scaffoldingOptions)
        : base(
            providerConfigurationCodeGenerator,
            annotationCodeGenerator,
            dbContextTemplateService,
            entityTypeTransformationService,
            cSharpHelper,
            options,
            scaffoldingOptions)
    {
        _options = options;
        _identityOptions = identityOptions;
    }

    protected override void GenerateClass(
        IModel model,
        string contextName,
        string connectionString,
        bool suppressConnectionStringWarning,
        bool suppressOnConfiguring)
    {
        TemplateData.Add("identity-key-type", CSharpHelper.Reference(model.FindIdentityKeyType(_identityOptions.Value.UserTableName)));

        base.GenerateClass(model, contextName, connectionString, suppressConnectionStringWarning, suppressOnConfiguring);

        var identityKeyType = model.FindIdentityKeyType(_identityOptions.Value.UserTableName);
        var lookup = _identityOptions.Value.CreateIdentityTableNameLookup(identityKeyType);

        var entities = model.GetScaffoldEntityTypes(_options.Value)
            .Where(e => !lookup.ContainsKey(e.Name))
            .ToList();

        TemplateData["dbsets"] = entities.ConvertAll(entityType => new Dictionary<string, object>
        {
            { "set-property-type", EntityTypeTransformationService.TransformTypeEntityName(entityType.Name) },
            { "set-property-name", entityType.GetDbSetName() },
        });
    }
}
