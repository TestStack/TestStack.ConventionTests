namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Linq;

    public class ConventionResult
    {
        public ConventionResult(Type dataType, string conventionTitle, string dataDescription, object[] data)
        {
            DataType = dataType;
            ConventionTitle = conventionTitle;
            DataDescription = dataDescription;
            Data = data;
        }

        public string RecommendedFileExtension { get; private set; }
        public Type DataType { get; private set; }
        public string ConventionTitle { get; private set; }
        public string DataDescription { get; private set; }
        public object[] Data { get; private set; }

        public bool HasData
        {
            get { return Data.Any(); }
        }

        public string FormattedResult { get; private set; }

        public void WithFormattedResult(string formattedResult, string recommendedFileExtension = "txt")
        {
            FormattedResult = formattedResult;
            RecommendedFileExtension = recommendedFileExtension;
        }
    }
}