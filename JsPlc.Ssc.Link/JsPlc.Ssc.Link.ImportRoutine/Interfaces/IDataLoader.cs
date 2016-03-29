using System.Data;

namespace JsPlc.Ssc.Link.ImportRoutine.Interfaces
{
	public interface IDataLoader
	{
		void Load(DataTable data);
	}
}
