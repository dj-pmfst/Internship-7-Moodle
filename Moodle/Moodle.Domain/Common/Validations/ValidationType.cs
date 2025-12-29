using System.Text.Json.Serialization;

namespace Moodle.Domain.Common.Validations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ValidationType
    {
        FormalValidation,
        BusinessRule,
        SystemError
    }
}
