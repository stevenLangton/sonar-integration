using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Js.Plc.Ssc.Link.Portal.BDD.Tests.Steps
{
    [Binding]
    public sealed class Hooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Create site object
            // Launch the browser through the IWebDriver (Chome Driver)

            // Login 
            
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Dispose "Site"
        }
    }
}
