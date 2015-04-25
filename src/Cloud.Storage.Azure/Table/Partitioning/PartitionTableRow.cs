namespace Cloud.Storage.Azure.Table.Partitioning
{
	public class PartitionTableRow : Row
	{
		public PartitionTableRow()
			: base()
		{
		}

		public PartitionTableRow(string partitionKey, string rowKey, bool hasSequentialRows = false, bool ascendingRowOrdering = false)
			: base(partitionKey, rowKey)
		{
			LastReadRowKey = string.Empty;
			HasSequentialRows = hasSequentialRows;
			AscendingRowOrdering = ascendingRowOrdering;
		}

		public string LastReadRowKey { get; set; }
		public bool HasSequentialRows { get; set; }
		public bool AscendingRowOrdering { get; set; }
	}
}
