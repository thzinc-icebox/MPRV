using System;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;

namespace MPRV.Model.Data
{
	public static class IDbCommandExecutor
	{
		#region Public Methods

		public static int ExecuteNonQuery(string connectionStringName, CommandType commandType, string query, ParameterList parameters)
		{
			int rowsAffected = 0;
			
			ConnectionStringSettings connectionInfo = ConfigurationManager.ConnectionStrings[connectionStringName];
			
			DbProviderFactory factory = DbProviderFactories.GetFactory(connectionInfo.ProviderName);

			// Configure connection
			using (IDbConnection connection = factory.CreateConnection())
			{
				connection.ConnectionString = connectionInfo.ConnectionString;
				connection.Open();

				// Build command
				IDbCommand command = connection.CreateCommand();
				command.CommandType = commandType;
				command.CommandText = query;
				command.AddParameters(parameters);

				rowsAffected = command.ExecuteNonQuery();
			}

			return rowsAffected;
		}

		public static DataSet ExecuteQuery(string connectionStringName, CommandType commandType, string query, ParameterList parameters, params string[] tableNames)
		{
			DataSet result = new DataSet();

			ConnectionStringSettings connectionInfo = ConfigurationManager.ConnectionStrings[connectionStringName];
			
			DbProviderFactory factory = DbProviderFactories.GetFactory(connectionInfo.ProviderName);

			// Configure connection
			IDbConnection connection = factory.CreateConnection();
			connection.ConnectionString = connectionInfo.ConnectionString;

			// Build command
			IDbCommand command = connection.CreateCommand();
			command.CommandType = commandType;
			command.CommandText = query;
			command.AddParameters(parameters);

			// Fill DataSet with result of command
			IDbDataAdapter dataAdapter = factory.CreateDataAdapter();
			dataAdapter.SelectCommand = command;
			dataAdapter.Fill(result);

			// Assign table names
			for (int i = 0; i < tableNames.Length && i < result.Tables.Count; i++)
			{
				result.Tables[i].TableName = tableNames[i];
			}

			return result;
		}

		#endregion

		#region Private Extension Methods

		public static void AddParameters(this IDbCommand command, ParameterList parameters)
		{
			if (command == null)
			{
				throw new ArgumentNullException();
			}

			foreach (var parameter in parameters)
			{
				var dbParameter = command.CreateParameter();
				dbParameter.ParameterName = parameter.Key;
				dbParameter.Value = parameter.Value;

				command.Parameters.Add(dbParameter);
			}
		}

		#endregion
	}
}

