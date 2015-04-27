using Cloud.Storage.Azure.Tables.Partitioning;
using Cloud.Storage.Tables;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Tables
{
	public class Table : ITable
	{
		public Table(CloudTable table)
		{
			AzureTable = table;
		}

		public string Name { get { return AzureTable.Name; } }

		public bool Exists()
		{
			return AzureTable.Exists();
		}

		public async Task InsertOrUpdateRows<T>(List<T> rows, bool forceOverwrite = false)
			where T : ITableEntity, new()
		{
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
						currentRow.ETag = "*";
						insertQuery.InsertOrReplace(currentRow);
					}
					else
					{
						currentRow.ETag = "*";
						insertQuery.InsertOrReplace(currentRow);
					}

					++currentRowNumber;

					if (currentRowNumber == 1000 || currentRowNumber == rowList.Count)
					{
						await AzureTable.ExecuteBatchAsync(insertQuery);
						insertQuery = new TableBatchOperation();
						currentRowNumber = 0;
					}
				}
			}
		}

		public async Task ExecuteActionOnAllRows<T>(Action<T> action)
			where T : ITableEntity, new()
		{
			var rows = new List<T>();

			TableContinuationToken continuationToken = null;
			var query = AzureTable.CreateQuery<T>();

			do
			{
				var segmentedQuery = await query.ExecuteSegmentedAsync(continuationToken);
				rows = segmentedQuery.ToList();

				foreach (var currentRow in rows)
				{
					action(currentRow);
				}

				if (segmentedQuery.ContinuationToken == null)
					break;

                continuationToken = segmentedQuery.ContinuationToken;
			}
			while (continuationToken != null);
		}

		public async Task<List<T>> GetAllRows<T>(bool usePartitionTable = true)
			where T : ITableEntity, new()
		{
			var rows = new List<T>();

			if (usePartitionTable)
			{
				var partitions = await TableStorageClient.PartitionTable.GetPartitionList(AzureTable.Name);
				foreach (var currentPartition in partitions)
				{
					rows.AddRange(await GetRowsByPartitionKey<T>(currentPartition.RowKey));
				}
			}
			else
			{
				TableContinuationToken continuationToken = null;
				var query = AzureTable.CreateQuery<T>();

				do
				{
					var segmentedQuery = await query.ExecuteSegmentedAsync(continuationToken);
					rows.AddRange(segmentedQuery.ToList());
					continuationToken = segmentedQuery.ContinuationToken;
				}
				while (continuationToken != null);
			}

			return rows;
		}

		public async Task<List<T>> GetNewRowsForPartition<T>(string partitionKey, Int64? fromValue = null, Int64? toValue = null)
			where T : ITableEntity, new()
		{
			var partition = await TableStorageClient.PartitionTable.GetPartition(AzureTable.Name, partitionKey);
			var latestRows = new List<T>();

			var rowStart = fromValue.HasValue ? fromValue.Value : partition.LastReadRowKey;
			var rowEnd = toValue.HasValue ? toValue.Value : (partition.AscendingRowOrdering ? Int64.MaxValue : 0);

			latestRows = await GetRowsByPartitionKey<T>(partitionKey, rowStart, rowEnd);

			Int64 lastRowRead = partition.LastReadRowKey;

			if (partition.AscendingRowOrdering)
			{
				if (latestRows.Count > 0)
				{
					lastRowRead = latestRows.Max(row => Int64.Parse(row.RowKey));
				}
			}
			else
			{
				if (latestRows.Count > 0)
				{
					lastRowRead = latestRows.Min(row => Int64.Parse(row.RowKey));
				}
			}

			partition.LastReadRowKey = lastRowRead;

			await TableStorageClient.PartitionTable.UpdatePartition(partition);

			return latestRows;
		}

		public async Task InsertOrUpdateRow<T>(T row, bool forceOverwrite = false)
			where T : class, IRow, ITableEntity, new()
		{
			await InsertOrUpdateRows(new List<T>() { row }, forceOverwrite);
		}

		public async Task<List<T>> GetRowsByPartitionKey<T>(string partitionKey, Int64? fromValue = null, Int64? toValue = null)
			where T : ITableEntity, new()
		{
			var rows = new List<T>();
			TableContinuationToken continuationToken = null;


			var rowStart = (fromValue.HasValue ? fromValue.Value : 0).ToStringComparableValue();
			var rowEnd = (toValue.HasValue ? toValue.Value : Int64.MaxValue).ToStringComparableValue();

			var query = AzureTable.CreateQuery<T>().Where(row => row.PartitionKey == partitionKey && row.RowKey.CompareTo(rowStart) > 0 ).AsTableQuery();

			do
			{
				var segmentedQuery = await query.ExecuteSegmentedAsync(continuationToken);
				rows.AddRange(segmentedQuery.ToList());
				continuationToken = segmentedQuery.ContinuationToken;
			}
			while (continuationToken != null);

			return rows;
		}

		public CloudTable AzureTable { get; private set; }
	}
}
