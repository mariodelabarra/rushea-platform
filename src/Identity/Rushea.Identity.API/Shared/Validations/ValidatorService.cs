using FluentValidation;
using FluentValidation.Results;

namespace Rushea.Identity.API.Shared.Validations;

public interface IValidationService
{
    Task<ValidationResult> ValidateAsync<T>(T instance, CancellationToken cancellationToken = default);
    Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default);
}

public class ValidationService(IServiceProvider serviceProvider) : IValidationService
{
    public async Task<ValidationResult> ValidateAsync<T>(T instance, CancellationToken cancellationToken = default)
    {
        var validator = serviceProvider.GetService<IValidator<T>>();
        return validator is null
            ? new ValidationResult()
            : await validator.ValidateAsync(instance, cancellationToken);
    }

    public async Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default)
    {
        var result = await ValidateAsync(instance, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }
}
