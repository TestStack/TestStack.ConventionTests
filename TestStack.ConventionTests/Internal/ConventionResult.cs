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

        protected bool Equals(ConventionResult other)
        {
            return DataType == other.DataType && string.Equals(ConventionTitle, other.ConventionTitle) && string.Equals(DataDescription, other.DataDescription);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConventionResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (DataType != null ? DataType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ConventionTitle != null ? ConventionTitle.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DataDescription != null ? DataDescription.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ConventionResult left, ConventionResult right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConventionResult left, ConventionResult right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("FormattedResult: {0}, DataDescription: {1}, ConventionTitle: {2}, DataType: {3}, HasData: {4}, RecommendedFileExtension: {5}", FormattedResult, DataDescription, ConventionTitle, DataType, HasData, RecommendedFileExtension);
        }
    }
}