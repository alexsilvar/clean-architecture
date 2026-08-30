namespace Domain.Audits;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class NoAuditAttribute : Attribute
{
}