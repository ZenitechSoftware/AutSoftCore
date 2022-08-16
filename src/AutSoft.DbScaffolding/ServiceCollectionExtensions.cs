using AutSoft.DbScaffolding.Configuration;
using AutSoft.DbScaffolding.Generators;
using AutSoft.DbScaffolding.Services;

using EntityFrameworkCore.Scaffolding.Handlebars;

using HandlebarsDotNet;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace AutSoft.DbScaffolding;

/// <summary>
/// Register all relevant services into dependency injection container
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "We do some low level stuff, so we take the risks.")]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register all relevant services into dependency injection container
    /// </summary>
    /// <returns>Expnaded service collection</returns>
    public static IServiceCollection AddDatabaseScaffolding(this IServiceCollection services, Action<DbScaffoldingOptions> dbConfigureOptions)
    {
        return services.AddDatabaseScaffoldingCore<Generators.CSharpEntityTypeGenerator, Generators.CSharpDbContextGenerator>(dbConfigureOptions);
    }

    /// <summary>
    /// Register all relevant services into dependency injection container with custom generators
    /// </summary>
    /// <typeparam name="TEntityGenerator">Custom entity generator implementation</typeparam>
    /// <typeparam name="TDbContextGenerator">Custom db context generator implementation</typeparam>
    /// <returns>Expnaded service collection</returns>
    public static IServiceCollection AddDatabaseScaffoldingCore<TEntityGenerator, TDbContextGenerator>(this IServiceCollection services, Action<DbScaffoldingOptions> dbConfigureOptions)
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
                HandlebarsScaffoldingOptions = options,
            });
        });

        services.Configure(dbConfigureOptions);

        services.AddHandlebarsHelpers();
        services.AddHandlebarsBlockHelpers();
        services.AddHandlebarsTransformers();

        services.AddSingleton<ICSharpEntityTypeGenerator, TEntityGenerator>();
        services.AddSingleton<ICSharpDbContextGenerator, TDbContextGenerator>();
        services.AddSingleton<IContextTransformationService, HbsContextTransformationService>();
        services.AddSingleton<IModelCodeGenerator, Generators.CSharpModelGenerator>();
        services.AddSingleton<IInterfaceGenerator, InterfaceGenerator>();
        services.AddSingleton<IInterfaceTemplateService, InterfaceTemplateService>();
        services.AddSingleton<ITemplateLanguageService, LanguageService>();

        return services;
    }
}
