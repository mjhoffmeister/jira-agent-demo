using System.Text.Json.Serialization;

namespace TaskApi.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskStatus
{
    Todo,
    InProgress,
    Done
}
