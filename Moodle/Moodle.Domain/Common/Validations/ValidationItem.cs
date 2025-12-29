namespace Moodle.Domain.Common.Validations
{
    public class ValidationItem
    {
        public ValidationSeverity ValidationSeverity { get; set; }
        public ValidationType ValidationType { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public ValidationItem(string code, string message, ValidationSeverity severity,
                      ValidationType type = ValidationType.BusinessRule)
        {
            Code = code;
            Message = message;
            ValidationSeverity = severity;
            ValidationType = type;
        }
    }
}