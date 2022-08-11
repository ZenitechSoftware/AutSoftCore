using EntityFrameworkCore.Scaffolding.Handlebars;
using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;

namespace AutSoft.DbScaffolding.EntityAbstractions.Services
{
    public class InterfaceTemplateService : HbsTemplateService, IInterfaceTemplateService
    {
        private readonly LanguageService _languageService;
        private Dictionary<string, TemplateFileInfo> InterfaceTemplateFiles { get; }

        public InterfaceTemplateService(ITemplateFileService fileService, ITemplateLanguageService languageService) : base(fileService, languageService)
        {
            _languageService = (LanguageService)languageService;
            InterfaceTemplateFiles = _languageService.GetInterfaceTemplateFileInfo();
        }

        public string GenerateInterface(Dictionary<string, object> data)
        {
            if (data == null)
                data = new Dictionary<string, object>();

            InterfaceTemplateFiles.TryGetValue(Consts.InterfaceTemplate, out TemplateFileInfo classFile);

            var entityTemplateFile = FileService.RetrieveTemplateFileContents(
                classFile.RelativeDirectory, classFile.FileName);

            var entityTemplate = Handlebars.Compile(entityTemplateFile);

            return entityTemplate(data);
        }

        protected override IDictionary<string, string> GetPartialTemplates(LanguageOptions language = LanguageOptions.CSharp)
        {
            InterfaceTemplateFiles.TryGetValue(Consts.InterfaceImportTemplate, out TemplateFileInfo importFile);
            var importTemplateFile = FileService.RetrieveTemplateFileContents(
                importFile.RelativeDirectory, importFile.FileName);

            InterfaceTemplateFiles.TryGetValue(Consts.InterfacePropertyTemplate, out TemplateFileInfo propertyFile);
            var propertyTemplateFile = FileService.RetrieveTemplateFileContents(
                propertyFile.RelativeDirectory, propertyFile.FileName);

            var templates = new Dictionary<string, string>
            {
                {
                    Consts.InterfaceImportTemplate.ToLower(),
                    importTemplateFile
                },
                {
                    Consts.InterfacePropertyTemplate.ToLower(),
                    propertyTemplateFile
                },
            };

            return templates;
        }
    }
}
