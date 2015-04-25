namespace Cloud.Storage.Tables
{
	public interface IRow
	{
		string PrimaryKey { get; }
		string SecondaryKey { get; }
	}

	public interface IRow<T> : IRow
	{
		T Data { get; set; }
	}
}
