using Moodle.Domain.Common.Validations;
using System.Text.RegularExpressions;
using ValidationResult = Moodle.Domain.Common.Validations.ValidationResult;

namespace Moodle.Application.Validators.Format
{
    public static class EmailValidator
    {
        private const int MinimumLength = 5;
        private const int MaximumLength = 150;

        public static ValidationResult Validate(string password)
        {
            var result = new ValidationResult();

            var errors = new List<string>();

            if (password.Length < MinimumLength)
            {
                errors.Add($"Lozinka mora sadržavati najmanje {MinimumLength} znakova.");
            }

            if (password.Length > MaximumLength)
            {
                errors.Add($"Lozinka ne smije biti duža od {MaximumLength} znakova.");
            }

            if (!HasUpperCase(password))
            {
                errors.Add("Lozinka mora sadržavati najmanje jedno veliko slovo.");
            }

            if (!HasLowerCase(password))
            {
                errors.Add("Lozinka mora sadržavati najmanje jedno malo slovo.");
            }

            if (!HasDigit(password))
            {
                errors.Add("Lozinka mora sadržavati najmanje jednu brojku.");
            }

            if (!HasSpecialCharacter(password))
            {
                errors.Add("Lozinka mora sadržavati najmanje jedan specijalni znak (!@#$%^&*()_+-=[]{}|;:,.<>?).");
            }

            foreach (var error in errors)
            {
                result.AddValidationItem(new ValidationItem(
                    code: "WeakPassword",
                    message: error,
                    severity: ValidationSeverity.Error
                ));
            }

            return result;
        }

        private static bool HasUpperCase(string password)
        {
            return Regex.IsMatch(password, @"[A-Z]");
        }

        private static bool HasLowerCase(string password)
        {
            return Regex.IsMatch(password, @"[a-z]");
        }

        private static bool HasDigit(string password)
        {
            return Regex.IsMatch(password, @"\d");
        }

        private static bool HasSpecialCharacter(string password)
        {
            return Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]");
        }
    }
}