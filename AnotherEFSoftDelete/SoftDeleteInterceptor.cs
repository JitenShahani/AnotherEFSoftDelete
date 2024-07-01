// List of EF Interceptors
// IInterceptor
// DbCommandInterceptor		-	IDbCommandInterceptor
// DbConnectionInterceptor	-	IDbConnectionInterceptor
// DbTransactionInterceptor	-	IDbTransactionInterceptor

namespace AnotherEFSoftDelete;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		if (eventData.Context is null) return result;

		foreach (EntityEntry entry in eventData.Context.ChangeTracker.Entries())
		{
			if (entry.Entity is ISoftDelete && entry.State == EntityState.Deleted)
			{
				entry.State = EntityState.Modified;
				entry.CurrentValues["DeletedOn"] = DateTime.Now;
			}
		}

		// Below code will only apply on Subscriber. Remove e.Entity is Subscriber to apply to all entities.
		foreach (EntityEntry entry in eventData.Context.ChangeTracker.Entries()
			.Where(e => e.Entity is Subscriber && e.State == EntityState.Added))
		{
			entry.Property("CreatedOn").CurrentValue = DateTime.Now;
		}

		return result;
	}
}