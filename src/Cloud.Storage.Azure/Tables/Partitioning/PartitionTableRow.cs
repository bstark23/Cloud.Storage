using System;

namespace Cloud.Storage.Azure.Tables.Partitioning
{
	public class PartitionTableRow : Row
	{
		public PartitionTableRow()
			: base()
		{
		}

		public PartitionTableRow(string partitionKey, string rowKey, bool hasSequentialRows = true, bool ascendingRowOrdering = true)
			: base(partitionKey, rowKey)
		{
			LastReadRowKey = ascendingRowOrdering ? 0 : Int64.MaxValue;
			HasSequentialRows = hasSequentialRows;
			AscendingRowOrdering = ascendingRowOrdering;
		}

		public Int64 LastReadRowKey { get; set; }
		public bool HasSequentialRows { get; set; }
		public bool AscendingRowOrdering { get; set; }
	}
}
