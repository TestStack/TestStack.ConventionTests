namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;

    public abstract class Convention<TData> : IConvention<TData> where TData : IConventionData
    {
        public abstract string ConventionTitle { get; }

        public virtual void Execute(TData data, IConventionResult result)
        {
            result.Is(Execute(data));
        }

        protected virtual IEnumerable<object> Execute(TData data)
        {
            throw new NotImplementedException("You need to overwrite the Execute method");
        }
    }
}