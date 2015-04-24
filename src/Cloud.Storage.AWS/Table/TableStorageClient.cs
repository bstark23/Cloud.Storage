using System;
using System.Threading.Tasks;
using Cloud.Storage.Table;

namespace Cloud.Storage.AWS.Table
{
	public class TableStorageClient : ITableStorageClient
	{
		public Task<ITable> GetTable(string tableName, bool createIfNotExists = false)
		{
			throw new NotImplementedException();
		}
	}
}
