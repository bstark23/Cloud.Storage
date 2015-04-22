using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Table.Partitioning
{
	public class PartitionTable
	{
		public static string PartitionTableName = "Partitions";

		static PartitionTable()
		{
			TableStorageClient.GetTable(PartitionTableName, true);
		}

		public static List<PartitionTableRow> GetPartitionList(string tableName)
		{
			var partitionList = TableStorageClient.GetRowsByPartitionKey<PartitionTableRow>(PartitionTableName, tableName);
			return partitionList.ToList();
		}

		public static PartitionTableRow GetPartition(string tableName, string partitionKey)
		{
			var partition = TableStorageClient.GetRowsByPartitionKey<PartitionTableRow>(PartitionTableName, tableName).SingleOrDefault(row => row.RowKey == partitionKey);
			return partition;
		}

		public static void UpdatePartition(PartitionTableRow partition)
		{
			TableStorageClient.InsertOrUpdateRowInTable(PartitionTableName, partition, true);
		}

		public static List<PartitionTableRow> GetNewPartitions(string tableName)
		{
			var partitionList = GetPartitionList(tableName);
			var lastPartition = partitionList.Count;
			var filterSB = new StringBuilder();

			if (lastPartition > 0)
			{
				filterSB.Append("(");
				for (var currRow = 0; currRow < partitionList.Count; ++currRow)
				{
					if (currRow != 0)
					{
						filterSB.AppendFormat(",PartitionKey!='{0}'", partitionList[currRow].RowKey);
					}
					else
					{
						filterSB.AppendFormat("PartitionKey!='{0}'", partitionList[currRow].RowKey);
					}
				}
				filterSB.Append(")");
			}

			TableStorageClient.ExecuteActionOnAllRows<TableEntity>(tableName,
				row =>
				{
					if (!partitionList.Any(partition => partition.RowKey == row.PartitionKey))
					{
						partitionList.Add(new PartitionTableRow(tableName, row.PartitionKey));
					}
				}, filterSB.ToString());

			return partitionList.Skip(lastPartition).ToList();
		}

		public static void UpdatePartitionTable(string tableName)
		{
			var newPartitions = GetNewPartitions(tableName);
			if (newPartitions.Any())
			{
				TableStorageClient.InsertOrUpdateRowsInTable(PartitionTableName, newPartitions);
			}
		}
	}
}
