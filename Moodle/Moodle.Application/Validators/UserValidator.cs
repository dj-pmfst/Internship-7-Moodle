using Moodle.Domain.Common.Validations;
using Moodle.Domain.Entities;

namespace Moodle.Application.Validators
{
    public class UserValidator
    {
        public static string CodePrefix = nameof(User);
        public const int MaxNameLength = 150;

        public static readonly ValidationItem NameMaxLength = new ValidationItem(
            code: $"{CodePrefix}1",
            message: $"Ime ne smije biti duže od {MaxNameLength} znakova.",
            severity: ValidationSeverity.Error,
            type: ValidationType.FormalValidation
        );

        public static readonly ValidationItem EmailInvalid = new ValidationItem(
            code: $"{CodePrefix}2",
            message: $"Email nije ispravnog formata.",
            severity: ValidationSeverity.Error,
            type: ValidationType.FormalValidation
         );
    }
}
