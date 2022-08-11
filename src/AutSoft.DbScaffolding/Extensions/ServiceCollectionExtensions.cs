using AutSoft.DbScaffolding.EntityAbstractions;
using AutSoft.DbScaffolding.EntityAbstractions.Services;
using EntityFrameworkCore.Scaffolding.Handlebars;
using EntityFrameworkCore.Scaffolding.Handlebars.Helpers;
using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutSoft.DbScaffolding.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseScaffolding(this IServiceCollection services, Action<DbScaffoldingOptions> dbConfigureOptions)
        {
            services.AddDatabaseScaffoldingCore<CSharpEntityTypeGenerator, CSharpDbContextGenerator>(dbConfigureOptions);
        }

        public static void AddDatabaseScaffoldingCore<TEntityGenerator, TDbContextGenerator>(this IServiceCollection services, Action<DbScaffoldingOptions> dbConfigureOptions)
            where TEntityGenerator : class, ICSharpEntityTypeGenerator
            where TDbContextGenerator : class, ICSharpDbContextGenerator
        {
            var generatorType = typeof(TEntityGenerator);
            var templateAssembly = generatorType.Assembly;
            var templateNamespace = generatorType.Namespace;

            services.AddHandlebarsScaffolding((options) =>
            {
                options.EmbeddedTemplatesAssembly = templateAssembly;
                options.EmbeddedTemplatesNamespace = templateNamespace;
                options.ReverseEngineerOptions = ReverseEngineerOptions.DbContextAndEntities;

                dbConfigureOptions(new DbScaffoldingOptions
                {
                    HandlebarsScaffoldingOptions = options
                });
            });

            services.Configure(dbConfigureOptions);

            services.AddHandlebarsHelpers();
            services.AddHandlebarsBlockHelpers();
            services.AddHandlebarsTransformers();

            services.AddSingleton<ICSharpEntityTypeGenerator, TEntityGenerator>();
            services.AddSingleton<ICSharpDbContextGenerator, TDbContextGenerator>();
            services.AddSingleton<IContextTransformationService, HbsContextTransformationService>();
            services.AddSingleton<IModelCodeGenerator, CSharpModelGenerator>();
            services.AddSingleton<IInterfaceGenerator, InterfaceGenerator>();
            services.AddSingleton<IInterfaceTemplateService, InterfaceTemplateService>();
            services.AddSingleton<ITemplateLanguageService, LanguageService>();
        }
    }
}
