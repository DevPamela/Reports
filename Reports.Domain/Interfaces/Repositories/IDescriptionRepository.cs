using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories
{
    public interface IDescriptionRepository
    {
        /// <summary>
        /// Add one assessment to the data store.
        /// </summary>
        /// <param name="description"></param>
        void Add(Description description);

        /// <summary>
        /// Retrieve all descriptions from the data store.
        /// </summary>
        /// <returns></returns>
        Task<List<Description>> GetAllAsync();

        /// <summary>
        /// Update a existent <see cref="description"/>.
        /// </summary>
        /// <param name="description"></param>
        void Update(Description description);

        /// <summary>
        ///  Retrieve one <see cref="Description"> from the data store by goal id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Description?> GetByGoalId(int id);
    }
}