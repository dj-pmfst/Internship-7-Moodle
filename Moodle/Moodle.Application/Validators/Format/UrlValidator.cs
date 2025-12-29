using Moodle.Domain.Common.Validations;

namespace Moodle.Moodle.Application.Validators.Format
{
    public class UrlValidator
    {
        public static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        public static ValidationResult Validate(string url)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(url))
            {
                result.AddError("UrlRequired", "URL required", ValidationType.FormalValidation);
                return result;
            }

            if (!IsValidUrl(url))
            {
                result.AddError("UrlInvalid", "Neispravan URL format", ValidationType.FormalValidation);
            }

            return result;
        }
    }
}
