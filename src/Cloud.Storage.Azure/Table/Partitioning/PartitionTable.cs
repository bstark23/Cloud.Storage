using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Table.Partitioning
{
	public class PartitionTable : Table
	{
		public PartitionTable(CloudTable partitions) 
			: base(partitions)
		{
		}

		public async Task<List<PartitionTableRow>> GetPartitionList(string tableName)
		{
			var partitionList = await GetRowsByPartitionKey<PartitionTableRow>(tableName);
			return partitionList.ToList();
		}

		public async Task<PartitionTableRow> GetPartition(string tableName, string partitionKey)
		{
			var partition = (await GetRowsByPartitionKey<PartitionTableRow>(tableName)).SingleOrDefault(row => row.RowKey == partitionKey);
			return partition;
		}

		public async Task UpdatePartition(PartitionTableRow partition)
		{
			await InsertOrUpdateRow(partition, true);
		}

		public async Task<List<PartitionTableRow>> GetNewPartitions(string tableName)
		{
			var partitionList = await GetPartitionList(tableName);
			var lastPartition = partitionList.Count;

			await ExecuteActionOnAllRows<TableEntity>(
				row =>
				{
					if (!partitionList.Any(partition => partition.RowKey == row.PartitionKey))
					{
						partitionList.Add(new PartitionTableRow(tableName, row.PartitionKey));
					}
				});

			return partitionList.Skip(lastPartition).ToList();
		}

		public async Task UpdatePartitionTable(string tableName)
		{
			var newPartitions = await GetNewPartitions(tableName);
			if (newPartitions.Any())
			{
				await InsertOrUpdateRows(newPartitions);
			}
		}
	}
}
