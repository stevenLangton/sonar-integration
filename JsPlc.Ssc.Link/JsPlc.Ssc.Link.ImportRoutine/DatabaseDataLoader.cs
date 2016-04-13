using JsPlc.Ssc.Link.ImportRoutine.Interfaces;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace JsPlc.Ssc.Link.ImportRoutine
{
	public class DatabaseDataLoader : IDataLoader
	{
		private Core.Interfaces.ILogger _logger;

		public DatabaseDataLoader(Core.Interfaces.ILogger logger)
		{
			// TODO: Complete member initialization
			this._logger = logger;
		}
		public void Load(DataTable data)
		{
			if (data == null)
				throw new ArgumentException("data");

			if (data.Rows.Count == 0)
				return;

			var connectionString = ConfigurationManager.ConnectionStrings["StubLinkColleagueRepository"].ConnectionString;

			//ORM to slow so using bulk import
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				try
				{
					_logger.Info("START Loading data");
					
					// Empty the destination tables. 
					SqlCommand deleteStubColleague = new SqlCommand(
						"TRUNCATE TABLE dbo.StubColleague;",
						connection);

					deleteStubColleague.ExecuteNonQuery();

					using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
					{
						sqlBulkCopy.ColumnMappings.Add(0, "ColleagueId");
						sqlBulkCopy.ColumnMappings.Add(1, "FirstName");
						sqlBulkCopy.ColumnMappings.Add(3, "KnownAsName");
						sqlBulkCopy.ColumnMappings.Add(2, "LastName");
						sqlBulkCopy.ColumnMappings.Add(6, "Grade");
						sqlBulkCopy.ColumnMappings.Add(8, "ManagerId");
						sqlBulkCopy.ColumnMappings.Add(7, "Division");
						sqlBulkCopy.ColumnMappings.Add(5, "Department");
						sqlBulkCopy.ColumnMappings.Add(4, "EmailAddress");

						sqlBulkCopy.DestinationTableName = "StubColleague";
						sqlBulkCopy.WriteToServer(data);
					}
					_logger.Info("END Data loaded");
				}
				catch(Exception ex)
				{
					_logger.Error("Can't upload data to database", ex);
				}
			}
		}
	}
}
