using System;
using System.Data;
using System.Configuration;
using System.Data.Common;

namespace MPRV.Model.Data
{
	public static class IDbCommandExecutor
	{
		public static DataSet ExecuteQuery (string connectionStringName, CommandType commandType, string query, params string[] tableNames)
		{
			DataSet result = new DataSet ();

			ConnectionStringSettings connectionInfo = ConfigurationManager.ConnectionStrings [connectionStringName];
			
			DbProviderFactory factory = DbProviderFactories.GetFactory (connectionInfo.ProviderName);

			// Configure connection
			IDbConnection connection = factory.CreateConnection ();
			connection.ConnectionString = connectionInfo.ConnectionString;

			// Build command
			IDbCommand command = connection.CreateCommand ();
			command.CommandType = commandType;
			command.CommandText = query;

			// Fill DataSet with result of command
			IDbDataAdapter dataAdapter = factory.CreateDataAdapter ();
			dataAdapter.SelectCommand = command;
			dataAdapter.Fill (result);

			// Assign table names
			for (int i = 0; i < tableNames.Length && i < result.Tables.Count; i++) {
				result.Tables[i].TableName = tableNames[i];
			}

			return result;
		}
	}
}

