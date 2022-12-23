using AutSoft.Common.Validation;

using FluentValidation;
using FluentValidation.Results;

using System.Linq.Expressions;

namespace AutSoft.AspNetCore.Blazor.Validation;

/// <inheritdoc />
public class FormValidatorWrapper<TDto> : IFormValidator<TDto>
{
    /// <inheritdoc />
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await _validator.ValidateAsync(ValidationContext<TDto>.CreateWithOptions((TDto)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };

    private readonly IValidator<TDto> _validator;

    /// <summary>
    /// Constructor of the FormValidatorWrapper.
    /// </summary>
    /// <param name="validator">Dto validator.</param>
    public FormValidatorWrapper(IValidator<TDto> validator)
    {
        _validator = validator;
    }

    /// <inheritdoc />
    public bool CanValidateInstancesOfType(Type type) => _validator.CanValidateInstancesOfType(type);

    /// <inheritdoc />
    public IValidatorDescriptor CreateDescriptor() => _validator.CreateDescriptor();

    /// <inheritdoc />
    public ValidationResult Validate(TDto instance) => _validator.Validate(instance);

    /// <inheritdoc />
    public ValidationResult Validate(IValidationContext context) => _validator.Validate(context);

    /// <inheritdoc />
    public Task<ValidationResult> ValidateAsync(TDto instance, CancellationToken cancellation = default) => _validator.ValidateAsync(instance, cancellation);

    /// <inheritdoc />
    public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default) => _validator.ValidateAsync(context, cancellation);

    /// <inheritdoc />
    public Func<T, Task<IEnumerable<string>>> GetValidatorFunc<T>(object model, string propertyName) =>
        _ => ValidateValue(model, propertyName);

    /// <inheritdoc />
    public Func<TU, Task<IEnumerable<string>>> GetValidatorFunc<T, TU>(T model, Expression<Func<T, TU>> propertyAccessor) =>
        _ => ValidateValue(model!, model.NameOf(propertyAccessor));
}
