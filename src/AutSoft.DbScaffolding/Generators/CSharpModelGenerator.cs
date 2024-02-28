using AutSoft.DbScaffolding.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using EntityFrameworkCore.Scaffolding.Handlebars;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding;
using EntityFrameworkCore.Scaffolding.Handlebars.Internal;

namespace AutSoft.DbScaffolding.Generators;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "We do some low level stuff, so we take the risks.")]
internal class CSharpModelGenerator : HbsCSharpModelGenerator
{
    private readonly IInterfaceGenerator _interfaceGenerator;
    private readonly DbScaffoldingOptions _options;

    public CSharpModelGenerator(
        ModelCodeGeneratorDependencies dependencies,
        ICSharpDbContextGenerator cSharpDbContextGenerator,
        ICSharpEntityTypeGenerator cSharpEntityTypeGenerator,
        IHbsHelperService handlebarsHelperService,
        IHbsBlockHelperService handlebarsBlockHelperService,
        IDbContextTemplateService dbContextTemplateService,
        IEntityTypeTemplateService entityTypeTemplateService,
        IEntityTypeTransformationService entityTypeTransformationService,
        IContextTransformationService contextTransformationService,
        ICSharpHelper cSharpHelper,
        IOptions<HandlebarsScaffoldingOptions> options,
        IInterfaceGenerator cSharpSoftDeleteInterfaceGenerator,
        IOptions<DbScaffoldingOptions> dbScaffoldingOptions)
        : base(
            dependencies,
            cSharpDbContextGenerator,
            cSharpEntityTypeGenerator,
            handlebarsHelperService,
            handlebarsBlockHelperService,
            dbContextTemplateService,
            entityTypeTemplateService,
            entityTypeTransformationService,
            contextTransformationService,
            cSharpHelper,
            options)
    {
        _interfaceGenerator = cSharpSoftDeleteInterfaceGenerator;
        _options = dbScaffoldingOptions.Value;
    }

    public override ScaffoldedModel GenerateModel(IModel model, ModelCodeGenerationOptions options)
    {
        var scaffoldedModel = base.GenerateModel(model, options);
        var entityTypes = model.GetEntityTypes();

        var hasDeletedPropertyEntity = entityTypes.Where(et => et.FindProperty(_options.InterfaceProperties.IsDeleted) != null);
        var hasAuditablePropertyEntity = entityTypes.Where(et => et.FindProperty(_options.InterfaceProperties.LastModAt) != null);

        if (hasDeletedPropertyEntity.Any())
        {
            // elég a First mert csak az interfész generálásához kell,
            // ahol nem számít melyik entitáson dolgozunk
            var generatedSoftDeletableInterfaceCode = _interfaceGenerator.GenerateSoftDeletableInterface(hasDeletedPropertyEntity.First(), options.UseNullableReferenceTypes);

            var scaffoldedSoftDeletebleInterface = new ScaffoldedFile
            {
                Path = $"{_options.InterfaceProperties.SoftDeletableInterfaceName}.cs",
                Code = generatedSoftDeletableInterfaceCode,
            };

            scaffoldedModel.AdditionalFiles.Add(scaffoldedSoftDeletebleInterface);
        }

        if (hasAuditablePropertyEntity.Any())
        {
            // elég a First mert csak az interfész generálásához kell,
            // ahol nem számít melyik entitáson dolgozunk
            var generatedAuditableInterfaceCode = _interfaceGenerator.GenerateAuditableInterface(hasAuditablePropertyEntity.First(), options.UseNullableReferenceTypes);

            var scaffoldedAuditableInterface = new ScaffoldedFile
            {
                Path = $"{_options.InterfaceProperties.AuditableInterfaceName}.cs",
                Code = generatedAuditableInterfaceCode,
            };

            scaffoldedModel.AdditionalFiles.Add(scaffoldedAuditableInterface);
        }

        return scaffoldedModel;
    }
}
