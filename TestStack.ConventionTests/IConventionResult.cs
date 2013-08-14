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
        /// <param name="firstSetFailureTitle">Title of the convention, i.e Dto's must live in Project.Dto namespace</param>
        /// <param name="firstSetFailureData">Data failing to conform to the convention</param>
        /// <param name="secondSetFailureTitle">The inverse scenario title, i.e Non-dtos must not live inside Project.Dto namespace</param>
        /// <param name="secondSetFailureData">Data failing to conform to the inverse of the convention</param>
        void IsSymmetric<TResult>(
            string firstSetFailureTitle, IEnumerable<TResult> firstSetFailureData,
            string secondSetFailureTitle, IEnumerable<TResult> secondSetFailureData);

        /// <summary>
        ///     A symmetric convention is a convention which also can be applied in reverse. For example
        ///     All dto's live in Project.Dto namespace AND Only dto's live in Project.Dto
        ///     This means if a DTO is outside of Project.Dto, the test will fail,
        ///     and if a non-dto is in Project.Dto the test will also fail
        /// 
        ///     This overload allows you to work with sets, see ....
        /// </summary>
        /// <typeparam name="TResult">The data type the convention is applied to</typeparam>
        /// <param name="firstSetFailureTitle">Title of the convention, i.e Dto's must live in Project.Dto namespace</param>
        /// <param name="secondSetFailureTitle">The inverse scenario title, i.e Non-dtos must not live inside Project.Dto namespace</param>
        /// <param name="allData">All data, for dto example, all types in the project, not just dto's</param>
        /// <param name="isPartOfFirstSet">Predicate defining data which is in the first set</param>
        /// <param name="isPartOfSecondSet">Predicate defining data which is in the second set</param>
        void IsSymmetric<TResult>(
            string firstSetFailureTitle,
            string secondSetFailureTitle,
            Func<TResult, bool> isPartOfFirstSet, 
            Func<TResult, bool> isPartOfSecondSet,
            IEnumerable<TResult> allData);
    }
}