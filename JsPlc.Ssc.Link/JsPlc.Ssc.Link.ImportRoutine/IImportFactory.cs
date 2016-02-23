using System.Collections.Generic;

namespace JsPlc.Ssc.Link.ImportRoutine
{
    public interface IImportFactory
    {
        List<ColleagueDto> ImportColleagueDataFromFile();

        void LoadDataIntoSqlServer(List<ColleagueDto> colleagueList);
    }
}
