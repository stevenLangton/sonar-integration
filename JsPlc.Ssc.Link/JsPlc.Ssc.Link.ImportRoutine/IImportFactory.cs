using System.Collections.Generic;

namespace JsPlc.Ssc.Link.ImportRoutine
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;

    public interface IImportFactory
    {
        List<ColleagueDto> ImportColleagueDataFromFile();

        void LoadDataIntoSqlServer(List<ColleagueDto> colleagueList);
    }
}
