using System.Threading.Tasks;

namespace Cloud.Storage.Table
{
	public interface ITableStorageClient
	{
		Task<ITable> GetTable(string tableName, bool createIfNotExists = false);

	}
}
