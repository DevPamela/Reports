using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories
{
    public interface IClassroomRepository
    {
        /// <summary>
        /// Add one <see cref="ClassRoom"> to the data store.
        /// </summary>
        /// <param name="classRoom"></param>
        void Add(ClassRoom classRoom);

        /// <summary>
        /// Retrieve all <see cref="ClassRoom"/> from data store.
        /// </summary>
        /// <returns></returns>
        Task<List<ClassRoom>> GetAllAsync();

        /// <summary>
        /// Retrieve one <see cref="ClassRoom"/>by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ClassRoom?> GetByNameAsync(string name);

        /// <summary>
        /// Retrieve one <see cref="ClassRoom"/> by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ClassRoom?> GetByIdAsync(int id);
    }
}
