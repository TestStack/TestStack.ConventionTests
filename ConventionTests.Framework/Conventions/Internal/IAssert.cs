namespace ConventionTests
{
    public interface IAssert
    {
        void Inconclusive(string message);
        void AreEqual(int expected, int actual, string message);
    }
}