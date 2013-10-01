using System.Reflection;
using System.Runtime.InteropServices;
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using TestStack.ConventionTests;
using TestStack.ConventionTests.Reporting;

[assembly: AssemblyTitle("TestStack.ConventionTests.Tests")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("TestStack.ConventionTests.Tests")]
[assembly: AssemblyCopyright("Copyright © TestStack 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("1a65a30e-e057-4058-a155-7ad12ba91cd2")]

[assembly: ConventionReporter(typeof(HtmlConventionResultsReporter))]
[assembly: ConventionReporter(typeof(MarkdownConventionResultsReporter))]