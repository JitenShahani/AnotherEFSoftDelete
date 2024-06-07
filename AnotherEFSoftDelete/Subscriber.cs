namespace AnotherEFSoftDelete;

public class Subscriber : ISoftDelete
{
	private string? _comment;

	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public DateTime? DeletedOn { get; set; }

	public void SetComment(string comment) => _comment = comment;
}