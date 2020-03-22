namespace TestStack.ConventionTests.ConventionData
{
    using System.Linq;

    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return value == null || value.All(char.IsWhiteSpace);
        }
    }
}