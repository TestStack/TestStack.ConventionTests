---

layout: layout

title: ConventionTests
---

## What is ConventionTests?

Convention over Configuration is a great way to cut down repetitive boilerplate code. But how do you validate that your code adheres to your conventions?

ConventionTests provides a simple API to build validation rules for creating convention validation tests.

## Getting Started
It is really easy to get started with ConventionTests, there are a number of included conventions that come out of the box:

 - All Classes Have Default Constructor 
 - All Methods are Virtual
 - Class type has specific namespace (for example, all dtos must live in the ProjectName.Dtos namespace)
 - Files are Embedded Resources
 - Project does not reference dlls from Bin or Obj directory
 - Others are being added over time and you can create your own custom ones

### Writing your first Convention test

#### 1. Using your favourite testing framework, create a new test. Lets call it:

`entities_must_have_default_constructor`

#### 2. Define some types to validate against a convention

The following line get a list of all the types in the assembly that contains `SampleDomainClass`.

    var itemsToVerify = Types.InAssemblyOf<SampleDomainClass>();
    
There are also overloads to restrict the types to a specific namespace within the assembly.

#### 3. Assert the convention

Now we have a list of types to check, we can use one of the pre-built conventions and check all of the types against this convention:

`Convention.Is(new AllClassesHaveDefaultConstructor(), itemsToVerify);`

#### That's it!

When you run this convention, if any of the types don't meet with the chosen convention, it fails and an exception will be thrown, which will look something like this:

	ConventionFailedException
	Message = Failed: 'Types must have a default constructor'
	-----------------------------------------------------------------
	
	TestAssembly.ClassWithNoDefaultCtor
	TestAssembly.ClassWithPrivateDefaultCtor

How cool is that!

### Reporting
If you would like to use ConventionTests reporting features, you just have to opt in by specifying the reporter you want. This makes it easy to add your own reporters, for example a WikiReporter may be better than the `HtmlReporter`

In your `Properties\AssemblyInfo.cs` file add the reporters you want. This are global reporters which will report the results of all conventions.

    [assembly: ConventionReporter(typeof(HtmlConventionResultsReporter))]
    [assembly: ConventionReporter(typeof(MarkdownConventionResultsReporter))]

Then if you look in the directory where your test assembly is, there will be an html report called `Conventions.htm`, serving as living documentation!