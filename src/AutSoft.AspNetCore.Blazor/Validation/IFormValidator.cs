using FluentValidation;

using System.Linq.Expressions;

namespace AutSoft.AspNetCore.Blazor.Validation;

/// <summary>
/// Defines a form validator for a type.
/// </summary>
/// <typeparam name="TDto">Type to validate.</typeparam>
public interface IFormValidator<TDto> : IValidator<TDto>
{
    /// <summary>
    /// Validate form value.
    /// </summary>
    Func<object, string, Task<IEnumerable<string>>> ValidateValue { get; }

    /// <summary>
    /// Get typed validator func.
    /// </summary>
    /// <typeparam name="T">Type of the func to return.</typeparam>
    Func<T, Task<IEnumerable<string>>> GetValidatorFunc<T>(object model, string propertyName);

    /// <summary>
    /// Get typed validator func.
    /// </summary>
    /// <typeparam name="T">Type of the func parameter.</typeparam>
    /// <typeparam name="TU">Type of the func to return.</typeparam>
    Func<TU, Task<IEnumerable<string>>> GetValidatorFunc<T, TU>(T model, Expression<Func<T, TU>> propertyAccessor);
}
