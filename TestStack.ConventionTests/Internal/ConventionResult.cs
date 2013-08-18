namespace TestStack.ConventionTests.Internal
{
    using System.Linq;

    public class ConventionResult
    {
        public ConventionResult(string conventionTitle, string dataDescription, object[] data)
        {
            ConventionTitle = conventionTitle;
            DataDescription = dataDescription;
            Data = data;
        }

        public string ConventionTitle { get; private set; }
        public string DataDescription { get; private set; }
        public object[] Data { get; private set; }

        public bool HasData
        {
            get { return Data.Any(); }
        }
    }
}