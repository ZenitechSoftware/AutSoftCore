using EntityFrameworkCore.Scaffolding.Handlebars;
using EntityFrameworkCore.Scaffolding.Handlebars.Helpers;
using System.Collections.Generic;

namespace AutSoft.DbScaffolding.EntityAbstractions.Services
{
    public class LanguageService : CSharpTemplateLanguageService
    {
        public Dictionary<string, TemplateFileInfo> GetInterfaceTemplateFileInfo()
        {
            var result = new Dictionary<string, TemplateFileInfo>
            {
                {
                    Consts.InterfaceTemplate,
                    new TemplateFileInfo
                    {
                        RelativeDirectory = Consts.InterfaceDirectory,
                        FileName = Consts.InterfaceTemplate + Constants.TemplateExtension
                    }
                },
                {
                    Consts.InterfaceImportTemplate,
                    new TemplateFileInfo
                    {
                        RelativeDirectory = Consts.InterfacePartialsDirectory,
                        FileName = Consts.InterfaceImportTemplate + Constants.TemplateExtension
                    }
                },
                {
                    Consts.InterfacePropertyTemplate,
                    new TemplateFileInfo
                    {
                        RelativeDirectory = Consts.InterfacePartialsDirectory,
                        FileName = Consts.InterfacePropertyTemplate + Constants.TemplateExtension
                    }
                },
            };
            return result;
        }
    }
}
