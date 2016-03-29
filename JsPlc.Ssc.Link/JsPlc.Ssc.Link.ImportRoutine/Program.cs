using JsPlc.Ssc.Link.Core;
using JsPlc.Ssc.Link.Core.Interfaces;
using JsPlc.Ssc.Link.ImportRoutine.Interfaces;
using Ninject;

namespace JsPlc.Ssc.Link.ImportRoutine
{
    class Program
    {
        static void Main(string[] args)
        {
			ILogger logger = new Log4NetLogger();
			IDataLoader dataLoader = new DatabaseDataLoader(logger);
			IDataTransformer dataTransformer = new DefaultTransformer();
			IFileProcessor fileProcessor = new DdatFileProcessor(logger);

			fileProcessor.ProcessFIMFile();
			fileProcessor.ProcessAbInitioFile();
			
			var processedData = fileProcessor.ProcessedData;

			var transformedData = dataTransformer.Transform(processedData);

			dataLoader.Load(transformedData);

			fileProcessor.MoveFilesToProcessedFolder();
        }
    }
}
