using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Interceptors;
public class AuditableEntityInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		UpdateEntities(eventData.Context);
		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		UpdateEntities(eventData.Context);
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	public void UpdateEntities(DbContext? context)
	{
		if (context == null) return;

		var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

		foreach (var entry in context.ChangeTracker.Entries<IEntity>())
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedBy = userId;
				entry.Entity.CreatedAt = DateTime.UtcNow;
			}

			if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
			{
				entry.Entity.LastModifiedBy = userId;
				entry.Entity.LastModifiedAt = DateTime.UtcNow;
			}
		}
	}
}

public static class Extensions
{
	public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
		entry.References.Any(r =>
			r.TargetEntry != null &&
			r.TargetEntry.Metadata.IsOwned() &&
			(r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
