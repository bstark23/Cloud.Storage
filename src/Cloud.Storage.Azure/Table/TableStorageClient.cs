using System;
using Cloud.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Cloud.Storage.Azure.Table.Partitioning;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Table
{
	public class TableStorageClient : ITableStorageClient
	{
		private static string PartitionTableName = "Partitions";

		public async Task<ITable> GetTable(string tableName, bool createIfNotExists = false)
		{
			var azureTable = await GetAzureTable(tableName, createIfNotExists);
			return new Table(azureTable);
		}

		public static async Task<CloudTable> GetAzureTable(string tableName, bool createIfNotExists = false)
		{
			var table = TableClient.GetTableReference(tableName);

			if (createIfNotExists)
			{
				await table.CreateIfNotExistsAsync();
			}

			return table;
		}

		//public static async Task<List<T>> GetRowsByPartitionKey<T>(string tableName, string partitionKey)
		//	where T : ITableEntity, new()
		//{
		//	var rows = new List<T>();
		//	var table = TableClient.GetTableReference(tableName);
		//	TableContinuationToken continuationToken = null;

		//	var query = table.CreateQuery<T>().Where(row => row.PartitionKey == partitionKey).AsTableQuery();

		//	do
		//	{
		//		var segmentedQuery = await query.ExecuteSegmentedAsync(continuationToken);
		//		rows.AddRange(segmentedQuery.ToList());
		//		continuationToken = segmentedQuery.ContinuationToken;
		//	}
		//	while (continuationToken != null);

		//	return rows;
		//}

		//public static async Task ExecuteActionOnAllRows<T>(string tableName, Action<T> action)
		//	where T : ITableEntity, new()
		//{
		//	var rows = new List<T>();
		//	var table = TableClient.GetTableReference(tableName);
		//	TableContinuationToken continuationToken = null;
		//	var query = table.CreateQuery<T>();

		//	do
		//	{
		//		var segmentedQuery = await query.ExecuteSegmentedAsync(continuationToken);
		//		rows = segmentedQuery.ToList();

		//		foreach (var currentRow in rows)
		//		{
		//			action(currentRow);
		//		}

		//		continuationToken = segmentedQuery.ContinuationToken;
		//	}
		//	while (continuationToken != null);
		//}

		//public static async Task<List<T>> GetAllRows<T>(string tableName, bool usePartitionTable = true)
		//	where T : ITableEntity, new()
		//{
		//	var rows = new List<T>();

		//	if (usePartitionTable)
		//	{
		//		var partitions = await PartitionTable.GetPartitionList(tableName);
		//		foreach (var currentPartition in partitions)
		//		{
		//			rows.AddRange(await GetRowsByPartitionKey<T>(tableName, currentPartition.RowKey));
		//		}
		//	}
		//	else
		//	{
		//		var table = TableClient.GetTableReference(tableName);
		//		TableContinuationToken continuationToken = null;
		//		var query = table.CreateQuery<T>();

		//		do
		//		{
		//			var segmentedQuery = await query.ExecuteSegmentedAsync(continuationToken);
		//			rows.AddRange(segmentedQuery.ToList());
		//			continuationToken = segmentedQuery.ContinuationToken;
		//		}
		//		while (continuationToken != null);
		//	}

		//	return rows;
		//}
		//public static async Task<List<T>> GetNewRowsForPartition<T>(string tableName, string partitionKey, bool ascendingRowKeys = false)
		//	where T : ITableEntity, new()
		//{
		//	var partition = await PartitionTable.GetPartition(tableName, partitionKey);
		//	var latestRows = new List<T>();

		//	latestRows = await GetRowsByPartitionKey<T>(tableName, partitionKey);
		//	if (!string.IsNullOrWhiteSpace(partition.LastReadRowKey))
		//	{
		//		if (ascendingRowKeys)
		//		{
		//			latestRows = latestRows.Where(row => int.Parse(row.RowKey) > int.Parse(partition.LastReadRowKey)).ToList();
		//		}
		//		else
		//		{
		//			latestRows = latestRows.Where(row => int.Parse(row.RowKey) < int.Parse(partition.LastReadRowKey)).ToList();

		//		}
		//	}

		//	var lastRowRead = !string.IsNullOrWhiteSpace(partition.LastReadRowKey) ? int.Parse(partition.LastReadRowKey) : 0;
		//	if (latestRows.Count > 0)
		//	{
		//		lastRowRead = latestRows.Max(row => int.Parse(row.RowKey));
		//	}
		//	partition.LastReadRowKey = lastRowRead.ToString();

		//	await PartitionTable.UpdatePartition(partition);

		//	return latestRows;
		//}

		//public static async Task InsertOrUpdateRowInTable<T>(string tableName, T row, bool forceOverwrite = false)
		//	where T : ITableEntity, new()
		//{
		//	await InsertOrUpdateRowsInTable(tableName, new List<T>() { row }, forceOverwrite);
		//}

		//public static async Task InsertOrUpdateRowsInTable<T>(string tableName, List<T> rows, bool forceOverwrite = false)
		//	where T : ITableEntity, new()
		//{
		//	var table = GetTable(tableName);
		//	var rowsByPartition = rows.GroupBy(row => row.PartitionKey).ToList();
		//	var insertQuery = new TableBatchOperation();
		//	int currentRowNumber = 0;

		//	foreach (var partitionedRows in rowsByPartition)
		//	{
		//		var rowList = partitionedRows.ToList();

		//		foreach (var currentRow in rowList)
		//		{
		//			currentRow.Timestamp = DateTime.UtcNow;
		//			if (!forceOverwrite)
		//			{
		//				insertQuery.Insert(currentRow);
		//			}
		//			else
		//			{
		//				currentRow.ETag = "*";
		//				insertQuery.InsertOrReplace(currentRow);
		//			}

		//			++currentRowNumber;

		//			if (currentRowNumber == 1000 || currentRowNumber == rowList.Count)
		//			{
		//				await table.ExecuteBatchAsync(insertQuery);
		//				insertQuery = new TableBatchOperation();
		//				currentRowNumber = 0;
		//			}
		//		}
		//	}
		//}
		public static CloudTableClient TableClient { get { return mTableClient.Value; } }
		public static PartitionTable PartitionTable { get { return mPartitionTable.Value; } }

		private static Lazy<CloudTableClient> mTableClient = new Lazy<CloudTableClient>(() => StorageClient.StorageAccount.CreateCloudTableClient());
		private static Lazy<PartitionTable> mPartitionTable = new Lazy<PartitionTable>(() => new PartitionTable(GetAzureTable(PartitionTableName, true).Result));

	}
}
