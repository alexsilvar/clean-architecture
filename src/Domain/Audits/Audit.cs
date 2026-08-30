using Domain.Audits.Enums;
using SharedKernel;

namespace Domain.Audits;

public sealed class Audit : Entity
{
    public Guid Id { get; private set; }

    public Guid CorrelationId { get; private set; }
    public AuditAction Action { get; private set; }
    public string EntityName { get; private set; }
    public string UserId { get; private set; }
    public DateTime DateTimeUtc { get; private set; }

    public Guid EntityId { get; private set; }

    public ICollection<AuditEntry>? ChangedValues { get; private set; } = new List<AuditEntry>();

    public ICollection<AffectedColumn>? ChangedProperties { get; private set; } = new List<AffectedColumn>();

    private Audit() { }

    public Audit(AuditAction action, string entityName, Guid entityId, string userId, DateTime timestamp)
    {
        Id = Guid.NewGuid();
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
        UserId = userId;
        DateTimeUtc = timestamp;
    }

    public void SetEntityId(Guid pk)
    {
        EntityId = pk;
    }

    public void SetCorrelationId(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public void AddChangedValue(string key, string value)
    {
        ChangedValues ??= [];
        ChangedValues?.Add(AuditEntry.Create(key, value));

        ChangedProperties ??= [];
        ChangedProperties?.Add(AffectedColumn.Create(key));
    }
}