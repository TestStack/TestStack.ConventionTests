namespace ConventionTests
{
    using System;
    using Castle.MicroKernel;
    using Castle.Windsor;

    public class WindsorConventionData
    {
        readonly IWindsorContainer container;

        public WindsorConventionData(IWindsorContainer container)
        {
            this.container = container;
        }

        public Func<IKernel, IHandler[]> Handlers { get; set; }

        public Func<IHandler, bool> Must { get; set; }

        public IKernel Kernel
        {
            get { return Container.Kernel; }
        }

        public IWindsorContainer Container
        {
            get { return container; }
        }

        public Func<IHandler, string> ItemDescription { get; set; }

        public string Description { get; set; }

        public bool HasApprovedExceptions { get; set; }
    }

    public class WindsorConventionData<TDiagnosticData>
    {
        readonly IWindsorContainer container;

        public WindsorConventionData(IWindsorContainer container)
        {
            this.container = container;
        }

        public IKernel Kernel
        {
            get { return Container.Kernel; }
        }

        public IWindsorContainer Container
        {
            get { return container; }
        }

        public Func<TDiagnosticData, string> ItemDescription { get; set; }

        public Func<TDiagnosticData, bool> Must { get; set; }

        public string Description { get; set; }

        public bool HasApprovedExceptions { get; set; }

        public Func<TDiagnosticData, object> OrderBy { get; set; }
    }
}