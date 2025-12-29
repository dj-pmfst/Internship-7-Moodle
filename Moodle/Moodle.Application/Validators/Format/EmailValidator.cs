using Moodle.Domain.Common.Validations;

namespace Moodle.Moodle.Application.Validators.Format
{
    public static class ValidatorEmail
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }

        public static ValidationResult Validate(string email) 
        { 
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(email))
            {
                result.AddError("EmailRequired", "Email required", ValidationType.FormalValidation);
                return result;
            }

            if (!IsValidEmail(email))
            {
                result.AddError("EmailInvalid", "Neispravan email format", ValidationType.FormalValidation);
            }

            return result;
        }
    }
}
