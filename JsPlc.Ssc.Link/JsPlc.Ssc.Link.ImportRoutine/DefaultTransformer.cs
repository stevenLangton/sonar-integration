using JsPlc.Ssc.Link.Core.Utils;
using JsPlc.Ssc.Link.ImportRoutine.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace JsPlc.Ssc.Link.ImportRoutine
{
	public class DefaultTransformer : IDataTransformer
	{
		public DataTable Transform(IReadOnlyCollection<ColleagueModel> processedData)
		{
			List<ColleagueModel> newList = new List<ColleagueModel>();

			foreach(var row in processedData)
			{
				newList.Add(row);
			}

			return DataTableUtils.ConvertToDataTable<ColleagueModel>(newList);
		}
	}
}
