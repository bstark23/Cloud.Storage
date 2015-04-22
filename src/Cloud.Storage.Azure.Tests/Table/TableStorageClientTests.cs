using Cloud.Storage.Azure.Table;
using Cloud.Storage.Azure.Table.Partitioning;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Azure.Storage.Tests.Table
{
	[TestFixture]
	public class TableStorageClientTests
	{
		public static string TestTableName = "Testing";
		public static int TestRowCount = 100;

		[Test]
		public void TestTableCreation()
		{
			var table = GetTestTable();
			Assert.IsTrue(table.Exists());
		}

		[Test]
		public void TestTableInsertion()
		{
			var rows = GenerateTestRows();
			TableStorageClient.InsertOrUpdateRowsInTable(TestTableName, rows, true);
		}

		[Test]
		public void TestPartitionTableCreation()
		{
			PartitionTable.UpdatePartitionTable(TestTableName);
		}

		[Test]
		public void AddMoreTestRows()
		{
			AddMoreRowsToExistingPartitions();
		}

		[Test]
		public void TestReadNewRowsSinceLastQuery()
		{
			AddMoreRowsToExistingPartitions();
			foreach (var partition in GetPartitionsForTestTable())
			{
				var latestRows = TableStorageClient.GetNewRowsForPartition<TableEntity>(TestTableName, partition.RowKey);
				Assert.IsTrue(latestRows.Count > 0);
				latestRows = TableStorageClient.GetNewRowsForPartition<TableEntity>(TestTableName, partition.RowKey);
				Assert.IsTrue(latestRows.Count == 0);
				AddMoreRowsToPartition(partition);
				latestRows = TableStorageClient.GetNewRowsForPartition<TableEntity>(TestTableName, partition.RowKey);
				Assert.IsTrue(latestRows.Count > 0);
			}
        }

		public static CloudTable GetTestTable()
		{
			var table = TableStorageClient.GetTable(TestTableName, true);
			return table;
		}
		public static System.Collections.Generic.List<TableEntity> GenerateTestRows()
		{
			var rows = new List<TableEntity>();
			for (int i = 0; i < TestRowCount; ++i)
			{
				rows.Add(
					new TableEntity()
					{
						PartitionKey = Guid.NewGuid().ToString(),
						RowKey = i.ToString(),
						ETag = "*",
						Timestamp = DateTime.UtcNow
					});
			}

			return rows;
		}

		public static void AddMoreRowsToExistingPartitions()
		{
			var partitionList = PartitionTable.GetPartitionList(TestTableName);
			var rows = new List<TableEntity>();

			foreach (var partition in partitionList)
			{
				AddMoreRowsToPartition(partition);
			}			
		}

		public static void AddMoreRowsToPartition(PartitionTableRow partition)
		{
			var rows = new List<TableEntity>();
			var partitionRows = TableStorageClient.
					GetRowsByPartitionKey<TableEntity>(TestTableName, partition.RowKey);

			var maxRowValue = partitionRows.Max(row => int.Parse(row.RowKey));
			for (int i = 0; i < TestRowCount; ++i)
			{
				++maxRowValue;
				rows.Add(new TableEntity() { PartitionKey = partition.RowKey, RowKey = maxRowValue.ToString(), ETag = "*", Timestamp = DateTime.UtcNow });
			}

			TableStorageClient.InsertOrUpdateRowsInTable(TestTableName, rows);
		}

		public static List<PartitionTableRow> GetPartitionsForTestTable()
		{
			var partitionList = PartitionTable.GetPartitionList(TestTableName);
			return partitionList;
		}
	}
}
