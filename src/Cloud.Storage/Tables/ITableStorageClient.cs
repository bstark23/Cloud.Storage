using System.Threading.Tasks;

namespace Cloud.Storage.Tables
{
	public interface ITableStorageClient
	{
		Task<ITable> GetTable(string tableName, bool createIfNotExists = false);

	}
}
