using System.Linq;

using AutSoft.DbScaffolding.EntityAbstractions;

using EntityFrameworkCore.Scaffolding.Handlebars;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.Options;

namespace AutSoft.DbScaffolding
{
    public class CSharpModelGenerator : HbsCSharpModelGenerator
    {
        protected readonly IInterfaceGenerator _interfaceGenerator;
        protected readonly DbScaffoldingOptions _options;

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
            : base(dependencies,
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
                    Code = generatedSoftDeletableInterfaceCode
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
                    Code = generatedAuditableInterfaceCode
                };

                scaffoldedModel.AdditionalFiles.Add(scaffoldedAuditableInterface);
            }

            return scaffoldedModel;
        }
    }
}
