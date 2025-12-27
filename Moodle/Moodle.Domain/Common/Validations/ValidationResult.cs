namespace Moodle.Domain.Common.Validations
{
    public class ValidationResult
    {
        private List<ValidationItem> _validationItems = new List<ValidationItem>();
        public IReadOnlyCollection<ValidationItem> ValidationItems => _validationItems;
        public bool HasError => _validationItems.Any(validationResult => validationResult.ValidationSeverity == ValidationSeverity.Error);
        public bool HasWarning => _validationItems.Any(validationResult => validationResult.ValidationSeverity == ValidationSeverity.Warning);
        public bool HasInfo => _validationItems.Any(validationResult => validationResult.ValidationSeverity == ValidationSeverity.Info);

        public void AddValidationItem(ValidationItem validationItem)
        {
            _validationItems.Add(validationItem);
        }
    }
}
