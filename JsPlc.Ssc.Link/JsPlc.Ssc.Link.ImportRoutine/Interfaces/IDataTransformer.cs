using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsPlc.Ssc.Link.ImportRoutine.Interfaces
{
	public interface IDataTransformer
	{
		DataTable Transform(IReadOnlyCollection<ColleagueModel> processedData);
	}
}
