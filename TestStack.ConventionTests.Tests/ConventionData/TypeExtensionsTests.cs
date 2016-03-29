namespace TestStack.ConventionTests.Tests.TestConventions
{
    using NUnit.Framework;

    using TestStack.ConventionTests.ConventionData;


    [TestFixture]
    public class TypeExtensionsTests
    {
        #region IsStatic

        [Test]
        public void IsStatic_StaticClass_True()
        {
            // Arrange
            var type = typeof(StaticClass);

            // Act
            var isStatic = type.IsStatic();

            // Assert
            Assert.That(isStatic, Is.True);
        }

        [Test]
        public void IsStatic_SealedClass_False()
        {
            // Arrange
            var type = typeof(SealedClass);

            // Act
            var isStatic = type.IsStatic();

            // Assert
            Assert.That(isStatic, Is.False);
        }

        [Test]
        public void IsStatic_NonStaticClass_False()
        {
            // Arrange
            var type = typeof(NonStaticClass);

            // Act
            var isStatic = type.IsStatic();

            // Assert
            Assert.That(isStatic, Is.False);
        }

        [Test]
        public void IsStatic_AbstractClass_False()
        {
            // Arrange
            var type = typeof(AbstractClass);

            // Act
            var isStatic = type.IsStatic();

            // Assert
            Assert.That(isStatic, Is.False);
        }

        [Test]
        public void IsStatic_Interface_False()
        {
            // Arrange
            var type = typeof(IInterface);

            // Act
            var isStatic = type.IsStatic();

            // Assert
            Assert.That(isStatic, Is.False);
        }

        [Test]
        public void IsStatic_SimpleType_False()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var isStatic = type.IsStatic();

            // Assert
            Assert.That(isStatic, Is.False);
        }


        private class NonStaticClass {}
        private sealed class SealedClass {}
        private static class StaticClass {}
        private abstract class AbstractClass {}
        private interface IInterface {}

        #endregion
    }
}