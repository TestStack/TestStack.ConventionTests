namespace TestStack.ConventionTests.Autofac
{
    /// <summary>
    /// Ordered by logical dependency precendence. Higher values should not reference lower
    /// </summary>
    public enum Lifetime
    {
        Transient = 0,
        InstancePerLifetimeScope = 1,
        SingleInstance = 2,
        ExternallyOwned = 3,
        SingleInstanceExternallyOwned = 4
    }
}