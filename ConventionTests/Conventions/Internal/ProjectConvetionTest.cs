namespace ConventionTests
{
    public abstract class ProjectConvetionTest : ConventionTestBase
    {
        public override void Execute(IAssert assert)
        {
            var data = SetUp();
        }

        /// <summary>
        ///     This is the only method you need to override. Return a <see cref="ConventionData" /> that describes your convention.
        /// </summary>
        /// <returns> </returns>
        protected abstract ProjectConventionData SetUp();
    }
}