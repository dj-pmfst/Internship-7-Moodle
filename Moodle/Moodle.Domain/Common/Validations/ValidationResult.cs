namespace Moodle.Domain.Common.Validations
{
    public class ValidationResult
    {
        private List<ValidationItem> _validationItems = new List<ValidationItem>();
        public IReadOnlyCollection<ValidationItem> ValidationItems => _validationItems;
        public bool HasError => _validationItems.Any(validationResult => validationResult.ValidationSeverity == ValidationSeverity.Error);
        public bool HasWarning => _validationItems.Any(validationResult => validationResult.ValidationSeverity == ValidationSeverity.Warning);
        public bool HasInfo => _validationItems.Any(validationResult => validationResult.ValidationSeverity == ValidationSeverity.Info);
        public bool IsValid => !HasError;

        public void AddValidationItem(ValidationItem validationItem)
        {
            _validationItems.Add(validationItem);
        }
        public void AddError(string code, string message, ValidationType type = ValidationType.BusinessRule)
        {
            _validationItems.Add(new ValidationItem(code, message, ValidationSeverity.Error, type));
        }

        public void AddWarning(string code, string message, ValidationType type = ValidationType.BusinessRule)
        {
            _validationItems.Add(new ValidationItem(code, message, ValidationSeverity.Warning, type));
        }

        public void AddInfo(string code, string message, ValidationType type = ValidationType.BusinessRule)
        {
            _validationItems.Add(new ValidationItem(code, message, ValidationSeverity.Info, type));
        }

        public IEnumerable<string> GetErrorMessages() =>
            _validationItems
                .Where(v => v.ValidationSeverity == ValidationSeverity.Error)
                .Select(v => v.Message);

        public IEnumerable<string> GetAllMessages() =>
            _validationItems.Select(v => v.Message);
    }
}
