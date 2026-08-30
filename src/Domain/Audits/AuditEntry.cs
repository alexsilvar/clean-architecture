using System.Text.Json;
using System.Text.Json.Serialization;
using SharedKernel;

namespace Domain.Audits;

public sealed class AuditEntry : ValueObject
{
    private static readonly JsonSerializerOptions JsonOptions = new() { Converters = { new JsonStringEnumConverter() } };

    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    private AuditEntry() { }

    public static AuditEntry Create(string key, string value)
    {
        return new AuditEntry { Key = key, Value = JsonSerializer.Serialize(value, JsonOptions) };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Key;
        yield return Value;
    }
}