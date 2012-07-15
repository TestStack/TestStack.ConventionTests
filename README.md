ConventionTests
===============

### What is ConventionTests?
Convention over Configuration is a great way to cut down repetitive boilerplate code. But how do you validate that your code adheres to your conventions? Convention Tests is a code-only NuGet that provides a simple API to build validation rules for convention validation tests.

### Using Con­ven­tion­Tests
Con­ven­tion­Tests is a sim­ple code-only Nuget that pro­vides a min­i­mal­is­tic and lim­ited API enforc­ing cer­tain struc­ture when writ­ing con­ven­tion tests and inte­grat­ing with NUnit. Installing it will add two .cs files to the project and a few depen­den­cies ([NUnit](http://www.nunit.org/), [Castle Wind­sor](http://stw.castleproject.org/Windsor.MainPage.ashx) and [ApprovalTests](http://approvaltests.sourceforge.net/)).

ConventionTests.NUnit file is where all the rel­e­vant code is located and __Run file is the file that runs your tests. The approach is to cre­ate a file per con­ven­tion and name them in a descrip­tive man­ner, so that you can learn what the con­ven­tions you have in the project are by just look­ing at the files in your Con­ven­tions folder, with­out hav­ing to open them.

Each con­ven­tion test inher­its (directly or indi­rectly) from the ICon­ven­tion­Test inter­face. There’s an abstract imple­men­ta­tion of the inter­face, Con­ven­tion­Test­Base and a few spe­cial­ized imple­men­ta­tions for com­mon sce­nar­ios pro­vided out of the box: Type-based one (Con­ven­tion­Test) and two for Wind­sor (Wind­sor­Con­ven­tion­Test, non-generic and generic for diagnostics-based tests).

#### Type-based con­ven­tion tests
The most com­mon and most generic group of con­ven­tions are ones based around types and type infor­ma­tion. Con­ven­tions like “every controller’s name ends with ‘Con­troller’”, or “Every method on WCF ser­vice con­tracts must have Oper­a­tionCon­trac­tAt­tribute” are exam­ples of such conventions.

You write them by cre­at­ing a class inher­it­ing Con­ven­tion­Test, which forces you to over­ride one method. Here’s a min­i­mal example

    public class Controllers_have_Controller_suffix_in_type_name : ConventionTest
    {
        protected override ConventionData SetUp()
        {
            return new ConventionData
                {
                    Types = t => t.IsConcrete<IController>(),
                    Must = t => t.Name.EndsWith("Controller")
                };
        }
    }

#### Windsor-based con­ven­tion tests
Another com­mon set of con­ven­tion tests are tests regard­ing an IoC con­tainer. Cas­tle Wind­sor is sup­ported out of the box. The struc­ture of the tests and API is sim­i­lar, with the dif­fer­ence being that instead of types we’re deal­ing with Windsor’s com­po­nent Han­dlers.

    public class List_classes_registered_in_Windsor : WindsorConventionTest
    {
        protected override WindsorConventionData SetUp()
        {
            return new WindsorConventionData(new WindsorContainer()
                                                    .Install(FromAssembly.Containing<AuditedAction>()))
                {
                    FailDescription = "All Windsor components",
                    FailItemDescription = h => BuildDetailedHandlerDescription(h)+" | "+
                        h.ComponentModel.GetLifestyleDescription(),
                }.WithApprovedExceptions("We just list all of them.");
     
        }
    }

#### Cus­tom con­ven­tion tests
Say we wanted to cre­ate a con­ven­tion test that lists all of our NHibernate col­lec­tions where we do cas­cade deletes, so that when we add a new col­lec­tion the test would fail remind­ing us of the issue, and force us to pay atten­tion to how we struc­ture rela­tion­ships in the appli­ca­tion. To do this we could cre­ate a base NHiber­nate­Con­ven­tion­Test and NHi­iber­nate­Con­ven­tion­Data to cre­ate sim­i­lar struc­ture, or just build a sim­ple one-class con­ven­tion like that:

    public class List_collection_that_cascade_deletes:ConventionTestBase
    {
        public override void Execute()
        {
            // NH Bootstrapper is our custom class to set up NH
            var bootstrapper = new NHibernateBootstrapper();
            var configuration = bootstrapper.BuildConfiguration();
     
            var message = new StringBuilder("Collections with cascade delete orphan");
            foreach (var @class in configuration.ClassMappings)
            {
                foreach (var property in @class.PropertyIterator)
                {
                    if(property.CascadeStyle.HasOrphanDelete)
                    {
                        message.AppendLine(@class.NodeName + "." + property.Name);
                    }
                }
            }
            Approve(message.ToString());
        }
    }

### Where to find out more
[Krzysztof Koźmic](https://github.com/kkozmic) spoke about ConventionTests at NDC 2012. You can find the video of that talk [here](http://vimeo.com/43676874), slides [here](http://kozmic.pl/presentations/) and the introductory blog post [here](http://kozmic.pl/2012/06/14/using-conventiontests/).