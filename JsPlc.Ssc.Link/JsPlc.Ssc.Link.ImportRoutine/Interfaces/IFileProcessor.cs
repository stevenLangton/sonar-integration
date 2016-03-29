using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsPlc.Ssc.Link.ImportRoutine.Interfaces
{
	public interface IFileProcessor
	{
		IReadOnlyCollection<ColleagueModel> ProcessedData { get; }

		void ProcessFIMFile();

		void ProcessAbInitioFile();

		void MoveFilesToProcessedFolder();
	}
}
