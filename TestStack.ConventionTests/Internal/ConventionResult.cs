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

        public Type DataType { get; private set; }
        public string ConventionTitle { get; private set; }
        public string DataDescription { get; private set; }
        public object[] Data { get; private set; }

        public bool HasData
        {
            get { return Data.Any(); }
        }

        protected bool Equals(ConventionResult other)
        {
            return DataType == other.DataType && string.Equals(ConventionTitle, other.ConventionTitle) && string.Equals(DataDescription, other.DataDescription);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
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
            return string.Format("DataType: {0}, ConventionTitle: {1}, DataDescription: {2}, HasData: {3}", DataType, ConventionTitle, DataDescription, HasData);
        }
    }
}