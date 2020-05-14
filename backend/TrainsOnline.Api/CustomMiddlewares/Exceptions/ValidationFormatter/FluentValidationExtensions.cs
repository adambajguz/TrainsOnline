namespace TrainsOnline.Api.CustomMiddlewares.Exceptions.ValidationFormatter
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;

    public static class FluentValidationExtensions
    {
        public static List<ValidationError> FormatValidationExceptions(this ValidationException validationException)
        {
            return validationException.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage, e.ErrorCode))
                                             .ToList();
        }
    }
}
