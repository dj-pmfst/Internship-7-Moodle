using Moodle.Domain.Common.Validations;

namespace Moodle.Moodle.Application.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public ValidationResult ValidationResult { get; }

        private ServiceResult(bool isSuccess, T? value, ValidationResult validationResult)
        {
            IsSuccess = isSuccess;
            Value = value;
            ValidationResult = validationResult;
        }

        public static ServiceResult<T> Success(T value)
        {
            return new ServiceResult<T>(true, value, new ValidationResult());
        }

        public static ServiceResult<T> Failure(ValidationResult validationResult)
        {
            return new ServiceResult<T>(false, default, validationResult);
        }

        public static ServiceResult<T> Failure(string errorMessage)
        {
            var validationResult = new ValidationResult();
            validationResult.AddError("Error", errorMessage);
            return new ServiceResult<T>(false, default, validationResult);
        }

        public static ServiceResult<T> Failure(string errorCode, string errorMessage)
        {
            var validationResult = new ValidationResult();
            validationResult.AddError(errorCode, errorMessage);
            return new ServiceResult<T>(false, default, validationResult);
        }
    }
}
