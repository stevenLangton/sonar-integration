
namespace JsPlc.Ssc.Link.ImportRoutine
{
    public class ImportData
    {
        IImportFactory objFactory = null;

        public ImportData(IImportFactory tmpFactory)
        {
            this.objFactory = tmpFactory;
        }

        public void DoImport()
        {
            this.objFactory.LoadDataIntoSqlServer(this.objFactory.ImportColleagueDataFromFile());
        }
    }
}
