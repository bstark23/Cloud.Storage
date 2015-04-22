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

namespace Cloud.Storage.Azure.Table
{
	public class TableStorageClient : ITableStorageClient
	{
		static TableStorageClient()
		{
		}

		public static CloudTable GetTable(string tableName, bool createIfNotExists = false)
		{
			var table = StorageClient.TableClient.GetTableReference(tableName);

			if (createIfNotExists)
			{
				table.CreateIfNotExists();
			}

			return table;
		}

		public static List<T> GetRowsByPartitionKey<T>(string tableName, string partitionKey)
			where T : ITableEntity, new()
		{
			var rows = new List<T>();
			var table = StorageClient.TableClient.GetTableReference(tableName);
			TableContinuationToken continuationToken = null;

			var query = table.CreateQuery<T>().Where(row => row.PartitionKey == partitionKey).AsTableQuery();

			do
			{
				var segmentedQuery = query.ExecuteSegmented(continuationToken);
				rows.AddRange(segmentedQuery.ToList());
				continuationToken = segmentedQuery.ContinuationToken;
			}
			while (continuationToken != null);

			return rows;
		}

		public static void ExecuteActionOnAllRows<T>(string tableName, Action<T> action, string filter = "")
			where T : ITableEntity, new()
		{
			var rows = new List<T>();
			var table = StorageClient.TableClient.GetTableReference(tableName);
			TableContinuationToken continuationToken = null;
			var query = table.CreateQuery<T>();

			if (!string.IsNullOrWhiteSpace(filter))
			{
				query = query.Where(filter);
			}

			do
			{
				var segmentedQuery = query.ExecuteSegmented(continuationToken);
				rows = segmentedQuery.ToList();

				foreach (var currentRow in rows)
				{
					action(currentRow);
				}

				continuationToken = segmentedQuery.ContinuationToken;
			}
			while (continuationToken != null);
		}

		public static List<T> GetAllRows<T>(string tableName, bool usePartitionTable = true)
			where T : ITableEntity, new()
		{
			var rows = new List<T>();

			if (usePartitionTable)
			{
				var partitions = PartitionTable.GetPartitionList(tableName);
				foreach (var currentPartition in partitions)
				{
					rows.AddRange(GetRowsByPartitionKey<T>(tableName, currentPartition.RowKey));
				}
			}
			else
			{
				var table = StorageClient.TableClient.GetTableReference(tableName);
				TableContinuationToken continuationToken = null;
				var query = table.CreateQuery<T>();

				do
				{
					var segmentedQuery = query.ExecuteSegmented(continuationToken);
					rows.AddRange(segmentedQuery.ToList());
					continuationToken = segmentedQuery.ContinuationToken;
				}
				while (continuationToken != null);
			}

			return rows;
		}
		public static List<T> GetNewRowsForPartition<T>(string tableName, string partitionKey)
			where T : ITableEntity, new()
		{
			var partition = PartitionTable.GetPartition(tableName, partitionKey);
			var latestRows = new List<T>();

			if (!string.IsNullOrWhiteSpace(partition.LastReadRowKey))
			{
				latestRows = GetRowsByPartitionKey<T>(tableName, partitionKey).Where(row => int.Parse(row.RowKey) > int.Parse(partition.LastReadRowKey)).ToList();
			}
			else
			{
				latestRows = GetRowsByPartitionKey<T>(tableName, partitionKey);
			}

			var lastRowRead = !string.IsNullOrWhiteSpace(partition.LastReadRowKey) ? int.Parse(partition.LastReadRowKey) : 0;
			if (latestRows.Count > 0)
			{
				lastRowRead = latestRows.Max(row => int.Parse(row.RowKey));
			}
			partition.LastReadRowKey = lastRowRead.ToString();

			PartitionTable.UpdatePartition(partition);

			return latestRows;
		}

		public static void InsertOrUpdateRowInTable<T>(string tableName, T row, bool forceOverwrite = false)
			where T : ITableEntity, new()
		{
			InsertOrUpdateRowsInTable(tableName, new List<T>() { row }, forceOverwrite);
		}

		public static void InsertOrUpdateRowsInTable<T>(string tableName, List<T> rows, bool forceOverwrite = false)
			where T : ITableEntity, new()
		{
			var table = GetTable(tableName);
			var rowsByPartition = rows.GroupBy(row => row.PartitionKey).ToList();
			var insertQuery = new TableBatchOperation();
			int currentRowNumber = 0;

			foreach (var partitionedRows in rowsByPartition)
			{
				var rowList = partitionedRows.ToList();

				foreach (var currentRow in rowList)
				{
					currentRow.Timestamp = DateTime.UtcNow;
					if (!forceOverwrite)
					{
						insertQuery.Insert(currentRow);
					}
					else
					{
						currentRow.ETag = "*";
						insertQuery.InsertOrReplace(currentRow);
					}

					++currentRowNumber;

					if (currentRowNumber == 1000 || currentRowNumber == rowList.Count)
					{
						table.ExecuteBatch(insertQuery).ToList();
						insertQuery = new TableBatchOperation();
						currentRowNumber = 0;
					}
				}
			}
		}
	}
}
