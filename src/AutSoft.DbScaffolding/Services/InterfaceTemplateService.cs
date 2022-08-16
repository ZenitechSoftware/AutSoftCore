using EntityFrameworkCore.Scaffolding.Handlebars;

using HandlebarsDotNet;

using Microsoft.EntityFrameworkCore.Design;

using System.Collections.Generic;
using System.Globalization;

namespace AutSoft.DbScaffolding.Services;

internal class InterfaceTemplateService : HbsTemplateService, IInterfaceTemplateService
{
    public InterfaceTemplateService(ITemplateFileService fileService, ITemplateLanguageService languageService)
        : base(fileService, languageService)
    {
        InterfaceTemplateFiles = ((LanguageService)languageService).GetInterfaceTemplateFileInfo();
    }

    private Dictionary<string, TemplateFileInfo> InterfaceTemplateFiles { get; }

    public string GenerateInterface(Dictionary<string, object> data)
    {
        data ??= new Dictionary<string, object>();

        InterfaceTemplateFiles.TryGetValue(Consts.InterfaceTemplate, out var classFile);

        var entityTemplateFile = FileService.RetrieveTemplateFileContents(
            classFile.RelativeDirectory, classFile.FileName);

        var entityTemplate = Handlebars.Compile(entityTemplateFile);

        return entityTemplate(data);
    }

    protected override IDictionary<string, string> GetPartialTemplates(LanguageOptions language = LanguageOptions.CSharp)
    {
        InterfaceTemplateFiles.TryGetValue(Consts.InterfaceImportTemplate, out var importFile);
        var importTemplateFile = FileService.RetrieveTemplateFileContents(
            importFile.RelativeDirectory, importFile.FileName);

        InterfaceTemplateFiles.TryGetValue(Consts.InterfacePropertyTemplate, out var propertyFile);
        var propertyTemplateFile = FileService.RetrieveTemplateFileContents(
            propertyFile.RelativeDirectory, propertyFile.FileName);

        return new Dictionary<string, string>
        {
            {
                Consts.InterfaceImportTemplate.ToLower(CultureInfo.InvariantCulture),
                importTemplateFile
            },
            {
                Consts.InterfacePropertyTemplate.ToLower(CultureInfo.InvariantCulture),
                propertyTemplateFile
            },
        };
    }
}
