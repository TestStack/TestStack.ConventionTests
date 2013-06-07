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

        public Func<IHandler, string> FailItemDescription { get; set; }

        public string FailDescription { get; set; }

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

        public Func<TDiagnosticData, string> FailItemDescription { get; set; }

        public Func<TDiagnosticData, bool> Must { get; set; }

        public string FailDescription { get; set; }

        public bool HasApprovedExceptions { get; set; }

        public Func<TDiagnosticData, object> OrderBy { get; set; }
    }
}