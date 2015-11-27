TestStack.ConventionTests
=========================

[![Build Status](https://ci.appveyor.com/api/projects/status/github/TestSTack/TestStack.ConventionTests?branch=master&svg=true)](https://ci.appveyor.com/project/TestStack/TestStack.ConventionTests) 
[![NuGet](https://img.shields.io/nuget/dt/TestStack.ConventionTests.svg)](https://www.nuget.org/packages/TestStack.ConventionTests) 
[![NuGet](https://img.shields.io/nuget/vpre/TestStack.ConventionTests.svg)](https://www.nuget.org/packages/TestStack.ConventionTests)

[![Join the chat at https://gitter.im/TestStack/TestStack.ConventionTests](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/TestStack/TestStack.ConventionTests?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

### What is ConventionTests?
Convention over Configuration is a great way to cut down repetitive boilerplate code. 
But how do you validate that your code adheres to your conventions? 
TestStack.ConventionTests is a simple NuGet library that makes it easy to build validation rules for convention validation tests.

TestStack.ConventionTests also will generate a convention report of the conventions present in your codebase, which you can publish as **living documentation**

### Using Con­ven­tion­Tests

    [Test]
    public void DomainHasVirtualMethodsConvention()
    {
        // Define some data
        var nHibernateEntities = Types.InAssemblyOf<SampleDomainClass>()
                .ConcreteTypes().InNamespace(typeof (SampleDomainClass).Namespace)
                .ToTypes("nHibernate Entitites");

        // Apply convention to data
        Convention.Is(new AllMethodsAreVirtual(), nhibernateEntities);
    }

For more information [view the TestStack.ConventionTests documentation](http://conventiontests.teststack.net/docs/)

### Packaged Conventions
Here is a list of some of the pacakged conventions

 - **AllClassesHaveDefaultConstructor** - All classes in defined data must have a default ctor (protected or public)
 - **AllMethodsAreVirtual** - All classes in defined data must have virtual methods (includes get_Property/set_Property backing methods)
 - **ClassTypeHasSpecificNamespace** - For example, Dto's must live in the Assembly.Dtos namespace'
 - **FilesAreEmbeddedResources** - All .sql files are embedded resources
 - **ProjectDoesNotReferenceDllsFromBinOrObjDirectories** - Specified project file must not reference assemblies from bin/obj directory
 - **MvcControllerNameAndBaseClassConvention** - Enforces MVC controller naming conventions
    - Types ending in *Controller must inherit from Controller (or ApiController), and
    - Types inheriting from ControllerBase must be named *Controller
 - **MvcControllerNameAndBaseClassConvention** - Enforces WebApi controller naming conventions
    - Types ending in *Controller must inherit from ApiController (or Controller), and
    - Types inheriting from ApiController must be named *Controller

If you would like to define your own conventions see [Defining Conventions](http://conventiontests.teststack.net/docs/getting-started#section-defining-conventions)

### More Information
[Krzysztof Koźmic](https://github.com/kkozmic) first spoke about ConventionTests at NDC 2012. You can find the video of that talk [here](http://vimeo.com/43676874), slides [here](http://kozmic.pl/presentations/) and the introductory blog post [here](http://kozmic.pl/2012/06/14/using-conventiontests/).

In v2, we have rewritten convention tests from the ground up to make it easier to get started, bundle some default conventions and also decouple it from a specific unit testing framework.

There is still plenty we can make better, so please raise issues on github with suggestions!

Docs are available at [TestStack.ConventionTests documentation](http://conventiontests.teststack.net/docs/)
