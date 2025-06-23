using Reports.Domain.Entities;
using Reports.Domain.Entities.Responses;

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
        Task<List<ClassRoom>> GetAllAsync(int teacherId);

        /// <summary>
        /// Retrieve one <see cref="ClassRoom"/>by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ClassRoom?> GetByNameAsync(string name, int teacherId);

        /// <summary>
        /// Retrieve one <see cref="ClassRoom"/> by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ClassRoom?> GetByIdAsync(int id);

        /// <summary>
        /// Check if a classroom exists for a teacher.
        /// </summary>
        /// <param name="classroomId">The Id of the classroom.</param>
        /// <param name="teacherId">The Id of the teacher.</param>
        /// <returns>True if the classroom exists for the teacher, false otherwise.</returns>
        Task<bool> ClassroomForTeacherExists(int classroomId, int teacherId);

        Task<List<ClassroomNameResponse>> GetClassroomNames(int teacherId);
        Task<List<ClassRoom>> GetClassroomByTeacherId(int teacherId);
    }
}