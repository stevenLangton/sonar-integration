using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Owin;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("JsPlc.Ssc.Link.Portal")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("JsPlc.Ssc.Link.Portal")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// OWIN Deployment Configurations
[assembly: OwinStartup("DevConfiguration", typeof(JsPlc.Ssc.Link.Portal.DevStartup))]
[assembly: OwinStartup("ProductionConfiguration", typeof(JsPlc.Ssc.Link.Portal.ProductionStartup))]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("c6be6515-fe21-4bdc-95fb-556706f33762")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")] // This is displayed in Link Footer..
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: InternalsVisibleTo("JsPlc.Ssc.Link.Portal.Tests")]

