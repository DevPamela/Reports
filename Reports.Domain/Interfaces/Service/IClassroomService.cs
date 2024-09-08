using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Service
{
    public interface IClassroomService
    {
        /// <summary>
        /// Add a new <see cref="ClassRoom">.
        /// </summary>
        /// <param name="classRoom"></param>
        /// <returns></returns>
        Task AddClassroom(ClassRoom classRoom);

        /// <summary>
        /// Set the <see cref="ClassRoom"> name, student list and teacher id.
        /// </summary>
        /// <param name="ClassroomName"></param>
        /// <param name="students"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Task SetClassRommInformation(string ClassroomName, List<Student> students, int teacherId);

        /// <summary>
        /// Get the <see cref="ClassRoom"> by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ClassRoom> GetById(int id);

        /// <summary>
        /// Get all classrooms in the database.
        /// </summary>
        /// <returns></returns>
        Task<List<ClassRoom>> GetAllAsync();

        /// <summary>
        /// Set IsSelected property as true.
        /// </summary>
        /// <param name="classroom"></param>
        /// <returns></returns>
        Task SetClassroomAsTrue(ClassRoom classroom);
    }
}
