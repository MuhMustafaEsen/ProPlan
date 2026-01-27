using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProPlan.Entities.ErrorModel;

namespace ProPlan.WebApi.Filters
{
    public class FluentValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var parameter in context.ActionDescriptor.Parameters)
            {
                if (context.ActionArguments.TryGetValue(parameter.Name, out var argument) && argument != null)
                {
                    var argumentType = argument.GetType();
                    var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                    var validator = _serviceProvider.GetService(validatorType);

                    if (validator != null)
                    {

                        var validationContextType = typeof(ValidationContext<>).MakeGenericType(argumentType);
                        var validationContext = Activator.CreateInstance(validationContextType, argument)!;

                        var validateMethod = validatorType.GetMethod("ValidateAsync", new[] { validationContextType, typeof(CancellationToken) });
                        if (validateMethod != null)
                        {
                            var task = validateMethod.Invoke(validator, new[] { validationContext, CancellationToken.None });
                            if (task is Task<ValidationResult> validationTask)
                            {
                                var validationResult = await validationTask;

                                if (!validationResult.IsValid)
                                {
                                    var errors = validationResult.Errors
                                        .GroupBy(e => e.PropertyName)
                                        .ToDictionary(
                                            g => g.Key,
                                            g => g.Select(e => e.ErrorMessage).ToArray()
                                        );

                                    var errorResponse = ErrorResponse.Create(
                                        400,
                                        "Validation failed",
                                        errors
                                    );

                                    context.Result = new BadRequestObjectResult(errorResponse);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            await next();
        }
    }
}
