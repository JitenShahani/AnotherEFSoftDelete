using AppDbContext context = new();

Subscriber subscriber1 = new() { Name = "Kelly"};
subscriber1.SetComment("EF is fun");
Subscriber subscriber2 = new() { Name = "Jacob" };
subscriber2.SetComment("Entity Framework is easy to learn but hard to master");

context.Set<Subscriber>().Add(subscriber1);
context.Set<Subscriber>().Add(subscriber2);
context.SaveChanges();

Subscriber subscriber = context.Set<Subscriber>().AsTracking().Single(s => s.Name == "Kelly");
context.Set<Subscriber>().Remove(subscriber);
context.SaveChanges();

List<Subscriber> subscribers = [.. context.Set<Subscriber>()];

WriteLine("\nFiltered Subscribers");
foreach (Subscriber sub in subscribers)
{
	WriteLine($"{sub.Name}, IsDeleted: {sub.DeletedOn.HasValue}");
}

List<Subscriber> allSubscribers = [..context.Set<Subscriber>().IgnoreQueryFilters()];

WriteLine("\nUnfiltered Subscribers");
foreach (Subscriber sub in allSubscribers)
{
	WriteLine($"{sub.Name}, IsDeleted: {sub.DeletedOn.HasValue}");
}

var data = context.Set<Subscriber>().IgnoreQueryFilters().Select(s => new
{
	s.Name,
	Comment = EF.Property<string>(s, "_comment"),
	Status = s.DeletedOn,
	Created = EF.Property<DateTime>(s, "CreatedOn")
});

WriteLine("\nDatabase Records");
foreach (var sub in data)
	WriteLine("\nName: {0}\nComment: {1}\nCreated: {2}\nDeleted: {3}\nDeleted On: {4}", sub.Name, sub.Comment, sub.Created, sub.Status.HasValue, sub.Status);