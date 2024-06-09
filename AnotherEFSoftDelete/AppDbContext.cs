namespace AnotherEFSoftDelete;

public class AppDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder
			.UseInMemoryDatabase("test")
			.AddInterceptors(new SoftDeleteInterceptor())
			.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Group statements based  on entities
		modelBuilder.Entity<Subscriber>(builder =>
		{
			// Create a Column for the field _comment
			builder
				.Property<string>("_comment")
				.HasColumnName("Comment");

			// Create a Shadow Column
			builder
				.Property<DateTime>("CreatedOn");

			builder
				.HasQueryFilter(subscriber => !subscriber.DeletedOn.HasValue);
		});
	}

	public DbSet<Subscriber> Subscribers { get; set; }
}