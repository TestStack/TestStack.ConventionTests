namespace SampleApp.Tests
{
    using NUnit.Framework;
    using SampleApp.Wpf;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class WpfTests
    {
        [Test]
        public void ViewModelCOnventions()
        {
            Convention.Is(new ViewModelShouldInheritFromINotifyPropertyChanged(), Types.InAssemblyOf<ViewModelBase>());
        }
    }
}