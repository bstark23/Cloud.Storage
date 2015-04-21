using System;
using Cloud.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table;
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

		public static List<T> GetRowsByPartitionKey<T>(string tableName, string partitionKey, string filter = "")
			where T : ITableEntity, new()
		{
			var rows = new List<T>();
			var table = StorageClient.TableClient.GetTableReference(tableName);
			TableContinuationToken continuationToken = null;

			var filterSB = new StringBuilder();
			filterSB.AppendFormat("(PartitionKey='{0}'");

			if (!string.IsNullOrWhiteSpace(filter))
			{
				filter = filter.Trim().Trim('(', ')');
				filterSB.AppendFormat(",{0}", filter);
			}

			filterSB.Append(")");

			var query = table.CreateQuery<T>().Where(filterSB.ToString());

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

		public static List<T> GetAllRows<T>(string tableName, bool usePartitionTable = true, string filter = "")
			where T : ITableEntity, new()
		{
			var rows = new List<T>();

			if (usePartitionTable)
			{
				var partitions = PartitionTable.GetPartitionList(tableName);
				foreach (var currentPartition in partitions)
				{
					rows.AddRange(GetRowsByPartitionKey<T>(tableName, currentPartition.RowKey, filter));
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

		public static void InsertRowsInTable<T>(string tableName, List<T> rows, bool forceOverwrite = false)
			where T : ITableEntity, new()
		{
			var table = GetTable(tableName);
			var insertQuery = new TableBatchOperation();
			int currentRowNumber = 0;

			foreach (var currentRow in rows)
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

				if (currentRowNumber == 1000)
				{
					table.ExecuteBatch(insertQuery).ToList();
					insertQuery = new TableBatchOperation();
					currentRowNumber = 0;
				}
			}
		}
	}
}
