using System;
using System.Configuration;
using System.Collections.Generic;

using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace JsPlc.Ssc.Link.ImportRoutine
{
    public class ImportFactory : IImportFactory
    {
        List<ColleagueDto> IImportFactory.ImportColleagueDataFromFile()
        {
            //Sample Test Data
            //string importPath = "C:/Temp/sample.ddat";

            //Real Test Data
            //string importPath = "C:/Temp/WFC_Cp_Hr_Info_Fim.ddat";

            //KEY to aquire URN value to ftp server
            string importPath = ConfigurationManager.AppSettings["UrnPath"];

            List<ColleagueDto> colleagueList = new List<ColleagueDto>();

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(importPath))
                {
                    string line;

                    // Read and display lines from the file until the end of the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        var colume = line.Split(Convert.ToChar("\x01"));

                        var colleague = new ColleagueDto
                        {

                            ColleagueId = colume[0],
                            FirstName = colume[1],
                            LastName = colume[3],
                            KnownAsName = colume[2],

                            Grade = colume[9],
                            ManagerId = colume[10],
                            Division = colume[23],
                            Department = colume[18]

                            //HasManager
                            //EmailAddress
                        };

                        colleagueList.Add(colleague);
                    }


                }

            }
            catch (Exception ex)
            {
                // Let the user know what went wrong.
                //Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(ex));
            }

            return colleagueList;
        }

        void IImportFactory.LoadDataIntoSqlServer(List<ColleagueDto> colleagueList)
        {
            DataTable dt = ConvertToDataTable(colleagueList);

            var connectionString = ConfigurationManager.ConnectionStrings["StubLinkColleagueRepository"].ConnectionString;

            //ORM to slow so using bulk import
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                // Empty the destination tables. 

                SqlCommand deleteStubColleague = new SqlCommand(
                    "DELETE FROM dbo.StubColleague;",
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

                    sqlBulkCopy.DestinationTableName = "StubColleague";
                    sqlBulkCopy.WriteToServer(dt);
                }
            }
        }

        private static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
    }
}
