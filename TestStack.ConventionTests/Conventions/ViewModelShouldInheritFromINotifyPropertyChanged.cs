namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using TestStack.ConventionTests.ConventionData;

    public class ViewModelShouldInheritFromINotifyPropertyChanged : IConvention<Types>
    {
        readonly string viewModelSuffix;

        public ViewModelShouldInheritFromINotifyPropertyChanged(string viewModelSuffix = "ViewModel")
        {
            this.viewModelSuffix = viewModelSuffix;
        }

        public void Execute(Types data, IConventionResultContext result)
        {
            var notifyPropertyChanged = typeof(INotifyPropertyChanged);
            var failingData = data.TypesToVerify.Where(t => t.Name.EndsWith(viewModelSuffix, StringComparison.OrdinalIgnoreCase))
                .Where(t => !notifyPropertyChanged.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()));

            result.Is($"ViewModels (types named *{viewModelSuffix}) should inherit from INotifyPropertyChanged",
                failingData);
        }

        public string ConventionReason => "In different scenarios, WPF can hold onto ViewModels if they do not inherit from INotifyPropertyChanged";
    }
}