namespace AnotherEFSoftDelete;

public interface ISoftDelete
{
	public DateTime? DeletedOn { get; set; }
}