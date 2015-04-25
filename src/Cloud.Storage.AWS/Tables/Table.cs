using Cloud.Storage.Tables;
using System;

namespace Cloud.Storage.AWS.Tables
{
	public class Table : ITable
	{
		public string Name
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool Exists()
		{
			throw new NotImplementedException();
		}
	}
}
