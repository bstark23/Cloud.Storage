using System;
using System.Threading.Tasks;
using Cloud.Storage.Tables;

namespace Cloud.Storage.AWS.Tables
{
	public class TableStorageClient : ITableStorageClient
	{
		public Task<ITable> GetTable(string tableName, bool createIfNotExists = false)
		{
			throw new NotImplementedException();
		}
	}
}
