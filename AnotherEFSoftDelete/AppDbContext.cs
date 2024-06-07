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
		// Group statements based  on entity
		modelBuilder.Entity<Subscriber>(builder =>
		{
			builder.HasQueryFilter(subscriber => !subscriber.DeletedOn.HasValue);

			// Field Property
			builder.Property<string>("_comment")
			.HasColumnName("Comment");

			// Shadow Property
			builder.Property<DateTime>("CreatedOn");
		});

		// modelBuilder
		// 	.Entity<Subscriber>()
		// 	.HasQueryFilter(subscriber => !subscriber.DeletedOn.HasValue);

		// Field Property
		// modelBuilder
		// 	.Entity<Subscriber>()
		// 	.Property<string>("_comment")
		// 	.HasColumnName("Comment");

		// Shadow Property
		// modelBuilder
		// 	.Entity<Subscriber>()
		// 	.Property<DateTime>("CreatedOn");
	}

	public DbSet<Subscriber> Subscribers { get; set; }
}