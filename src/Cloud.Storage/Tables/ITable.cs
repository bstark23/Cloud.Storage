using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloud.Storage.Tables
{
	public interface ITable
	{
		string Name { get; }

		bool Exists();

		//Task<List<T>> GetAllRows<T>()
		//	where T : class, IRow, new();

		//Task<T> GetRow<T>(string primaryKey, string secondaryKey)
		//	where T : class, IRow, new();

		//Task<List<T>> GetRowsByPrimaryKey<T>(string primaryKey)
		//	where T : class, IRow, new();

		//Task InsertOrUpdateRow<T>(T row, bool forceOverride = false)
		//	where T : class, IRow, new();

		//Task InsertOrUpdateRows<T>(List<T> rows, bool forceOverride = false)
		//	where T : class, IRow, new();

		//Task DeleteRow<T>(T row)
		//	where T : class, IRow, new();

		//Task DeleteRow(string primaryKey, string secondaryKey);
		//Task DeleteRow(RowReference row);

		//Task DeleteRows<T>(List<T> rows)
		//	where T : class, IRow, new();

		//Task DeleteRows(List<RowReference> rows);

	}
}
