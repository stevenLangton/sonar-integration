using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using Ninject;


namespace JsPlc.Ssc.Link.ImportRoutine.Service
{
    public partial class ImportService : ServiceBase
    {
        private System.Diagnostics.EventLog eventLog1;

        public ImportService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("LinkImportService"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "LinkImportService", "LinkLog");
            }
            eventLog1.Source = "LinkImportService";
            eventLog1.Log = "LinkLog";

        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");

            Ninject.IKernel kernal = new StandardKernel();
            kernal.Bind<IImportFactory>().To<ImportFactory>();

            var instance = kernal.Get<ImportData>();
            instance.DoImport();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In onStop.");
        }
    }
}
