using Microsoft.WindowsAzure.Storage.Table;

namespace Cloud.Storage.Azure.Table.Partitioning
{
	public class PartitionTableRow : TableEntity
	{
		public PartitionTableRow()
			: base()
		{
		}

		public PartitionTableRow(string partitionKey, string rowKey)
			: base(partitionKey, rowKey)
		{
			LastReadRowKey = string.Empty;
		}

		public string LastReadRowKey { get; set; }
	}
}
