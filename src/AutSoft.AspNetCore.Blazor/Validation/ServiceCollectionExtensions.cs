using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace AutSoft.AspNetCore.Blazor.Validation;

/// <summary>
/// Form validator DI config.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add <see cref="IFormValidator{TDto}">IFormValidator</see> and <see cref="FormValidatorWrapper{TDto}">FormValidatorWrapper</see> to the DI services.
    /// </summary>
    public static void AddFormValidatorsForValidators(this IServiceCollection services)
    {
        foreach (var validator in services.Where(d => d.ServiceType.IsAssignableTo(typeof(IValidator))).ToList())
        {
            var args = validator.ServiceType.GetGenericArguments();

            if (args.Length == 0)
                continue;

            services.Add(new ServiceDescriptor(
                typeof(IFormValidator<>).MakeGenericType(args[0]),
                typeof(FormValidatorWrapper<>).MakeGenericType(args[0]),
                validator.Lifetime));
        }
    }
}
