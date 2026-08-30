using Application.Abstractions.Authentication;
using Domain.Audits;
using Domain.Audits.Enums;
using Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Infrastructure.Database;

internal sealed class AuditInterceptor(
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        AddAudits(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AddAudits(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AddAudits(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var audits = context.ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity is not Audit &&
                            entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Select(CreateAudit)
            .ToList();

        context.Set<Audit>().AddRange(audits);
    }

    private Audit CreateAudit(EntityEntry<Entity> entry)
    {
        AuditAction action = entry.State switch
        {
            EntityState.Added => AuditAction.Create,
            EntityState.Deleted => AuditAction.Delete,
            _ => AuditAction.Update
        };

        Guid entityId = GetEntityId(entry);
        var audit = new Audit(action, entry.Metadata.ClrType.Name, entityId, GetUserId(), dateTimeProvider.UtcNow);

        foreach (PropertyEntry property in entry.Properties.Where(property =>
                     property.Metadata.PropertyInfo?.IsDefined(typeof(NoAuditAttribute), inherit: true) != true))
        {
            object? value = action == AuditAction.Delete ? property.OriginalValue : property.CurrentValue;

            if (action != AuditAction.Update || !Equals(property.OriginalValue, property.CurrentValue))
            {
                audit.AddChangedValue(property.Metadata.Name, value is null ? "null" : value.ToString()!);
            }
        }

        return audit;
    }

    private string GetUserId()
    {
        try
        {
            return userContext.UserId.ToString();
        }
        catch (UserContextUnavailableException)
        {
            return "anonymous";
        }
        catch (ApplicationException)
        {
            return "anonymous";
        }
    }

    private static Guid GetEntityId(EntityEntry<Entity> entry)
    {
        PropertyEntry? idProperty = entry.Properties.FirstOrDefault(property => property.Metadata.IsPrimaryKey());
        return idProperty?.CurrentValue is Guid id ? id : Guid.Empty;
    }
}