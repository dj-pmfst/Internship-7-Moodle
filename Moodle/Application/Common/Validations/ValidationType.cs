using System.Text.Json.Serialization;

namespace Moodle.Application.Common.Validations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ValidationType
    {
        FormalValidation,
        BussinessRule,
        SystemError
    }
}
