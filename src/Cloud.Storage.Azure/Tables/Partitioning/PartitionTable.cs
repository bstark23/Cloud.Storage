using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Tables.Partitioning
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

		public async Task UpdatePartition(PartitionTableRow partition, bool forceOverwrite = true)
		{
			try
			{
				await InsertOrUpdateRow(partition, forceOverwrite, false);
			}
			catch
			{
				//Just blindly eating exception here while I try and figure out the best way of only inserting new partitions without a bunhc of queries.
			}
		}

		public async Task<List<PartitionTableRow>> GetNewPartitions(string tableName)
		{
			var partitionList = await GetPartitionList(tableName);
			var lastPartition = partitionList.Count;
			var table = await StorageClient.CloudStorage.Tables.GetTable(tableName) as Table;

			await table.ExecuteActionOnAllRows<TableEntity>(
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
				await InsertOrUpdateRows(newPartitions, true, false);
			}
		}
	}
}
