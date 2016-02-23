using Ninject;

namespace JsPlc.Ssc.Link.ImportRoutine
{

    class ColleagueImportRoutine
    {
        static void Main(string[] args)
        {
            //var watch = Stopwatch.StartNew();

            Ninject.IKernel kernal = new StandardKernel();  
            kernal.Bind<IImportFactory>().To<ImportFactory>(); 
 
            var instance = kernal.Get<ImportData>();  
            instance.DoImport();

            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
        }
    }

  
}
