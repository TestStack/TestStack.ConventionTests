namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;

    public interface IConventionResult
    {
        void Is<T>(string resultTitle, IEnumerable<T> failingData);

        /// <summary>
        ///     A symmetric convention is a convention which also can be applied in reverse. For example
        ///     All dto's live in Project.Dto namespace AND Only dto's live in Project.Dto
        ///     This means if a DTO is outside of Project.Dto, the test will fail,
        ///     and if a non-dto is in Project.Dto the test will also fail
        /// </summary>
        /// <typeparam name="TResult">The data type the convention is applied to</typeparam>
        /// <param name="conventionResultTitle">Title of the convention, i.e Dto's must live in Project.Dto namespace</param>
        /// <param name="conventionFailingData">Data failing to conform to the convention</param>
        /// <param name="inverseResultTitle">The inverse scenario title, i.e Non-dtos must not live inside Project.Dto namespace</param>
        /// <param name="inverseFailingData">Data failing to conform to the inverse of the convention</param>
        void IsSymmetric<TResult>(
            string conventionResultTitle, IEnumerable<TResult> conventionFailingData,
            string inverseResultTitle, IEnumerable<TResult> inverseFailingData);

        /// <summary>
        ///     A symmetric convention is a convention which also can be applied in reverse. For example
        ///     All dto's live in Project.Dto namespace AND Only dto's live in Project.Dto
        ///     This means if a DTO is outside of Project.Dto, the test will fail,
        ///     and if a non-dto is in Project.Dto the test will also fail
        /// </summary>
        /// <typeparam name="TResult">The data type the convention is applied to</typeparam>
        /// <param name="conventionResultTitle">Title of the convention, i.e Dto's must live in Project.Dto namespace</param>
        /// <param name="inverseResultTitle">The inverse scenario title, i.e Non-dtos must not live inside Project.Dto namespace</param>
        /// <param name="isInclusiveData">Predicate describing if the data is included, for example, t => t.Name.EndsWith("Dto")</param>
        /// <param name="dataConformsToConvention">
        ///     Predicate describing the convention, for example, t =>
        ///     t.NameSpace.StartsWith("Project.Dto")
        /// </param>
        /// <param name="allData">All data, for dto example, all types in the project, not just dto's</param>
        void IsSymmetric<TResult>(string conventionResultTitle, string inverseResultTitle, Func<TResult, bool> isInclusiveData, Func<TResult, bool> dataConformsToConvention, IEnumerable<TResult> allData);
    }
}