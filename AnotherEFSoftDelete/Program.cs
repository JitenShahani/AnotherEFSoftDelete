using AppDbContext context = new();

Subscriber subscriber1 = new() { Name = "Kelly"};
subscriber1.SetComment("EF is fun");
Subscriber subscriber2 = new() { Name = "Jacob" };
subscriber2.SetComment("Entity Framework is easy to learn but hard to master");

context.Subscribers.Add(subscriber1);
context.Subscribers.Add(subscriber2);
context.SaveChanges();

Subscriber subscriber = context.Subscribers.AsTracking().Single(s => s.Name == "Kelly");
context.Subscribers.Remove(subscriber);
context.SaveChanges();

List<Subscriber> subscribers = [.. context.Subscribers];

WriteLine("\nFiltered Subscribers");
foreach (Subscriber sub in subscribers)
{
	WriteLine($"{sub.Name}, IsDeleted: {sub.DeletedOn.HasValue}");
}

List<Subscriber> allSubscribers = [..context.Subscribers.IgnoreQueryFilters()];

WriteLine("\nUnfiltered Subscribers");
foreach (Subscriber sub in allSubscribers)
{
	WriteLine($"{sub.Name}, IsDeleted: {sub.DeletedOn.HasValue}");
}

var data = context.Subscribers.IgnoreQueryFilters().Select(s => new
{
	s.Name,
	Comment = EF.Property<string>(s, "_comment"),
	Status = s.DeletedOn,
	Created = EF.Property<DateTime>(s, "CreatedOn")
});

WriteLine("\nDatabase Records");
foreach (var sub in data)
	WriteLine("\nName: {0}\nComment: {1}\nCreated: {2}\nDeleted: {3}\nDeleted On: {4}", sub.Name, sub.Comment, sub.Created, sub.Status.HasValue, sub.Status);