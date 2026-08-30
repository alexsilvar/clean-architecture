using SharedKernel;

namespace Domain.Audits;

public sealed class AffectedColumn : ValueObject
{
    public string Column { get; private set; } = string.Empty;

    private AffectedColumn() { }

    public static AffectedColumn Create(string column)
    {
        return new AffectedColumn { Column = column };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Column;
    }
}
