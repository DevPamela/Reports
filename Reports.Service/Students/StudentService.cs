using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Entities;

namespace Reports.Service.Students;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBulkInsertHelper _bulkInsertHelper;

    public StudentService(
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork,
        IBulkInsertHelper bulkInsertHelper)
    {
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _bulkInsertHelper = bulkInsertHelper;
    }

    public async Task UpdateStudent(Student student)
    {
        try
        {
            var repo = await _studentRepository.GetByNameAsync(student.Name);
            repo.Gender = student.Gender;
            repo.Name = student.Name;
            if(student.ClassRoomId != 0)
            {
                repo.ClassRoomId = student.ClassRoomId;
            }

            if (student.Birthdate != DateOnly.MinValue && student.Birthdate < DateOnly.FromDateTime(DateTime.Now))
                repo.Birthdate = student.Birthdate;

            _studentRepository.Update(repo);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// Gets the student data by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Student> GetByIdAsync(int id) =>
        await _studentRepository.GetByIdAsync(id);

    /// <summary>
    /// Gets student list.
    /// </summary>
    /// <returns></returns>
    public async Task<List<Student>> GetStudentsListAsync() =>
        await _studentRepository.GetAllAsync();

    public async Task<List<Student>> GetStudentByIsActiveAsync() =>
        await _studentRepository.GetStudentByIsActiveAsync();

    /// <summary>
    /// Gets student data by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<Student?> GetStudentByNameAsync(string name) =>
        await _studentRepository.GetByNameAsync(name);

    /// <summary>
    /// Clears the "IsSelected" property.
    /// </summary>
    /// <param name="students"></param>
    /// <returns></returns>
    public async Task ClearSelection(List<Student> students)
    {
        SelectableEntity<Student> selectableSomeEntity = new SelectableEntity<Student>();
        foreach (var student in students)
        {
            selectableSomeEntity.ClearSelection(student);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Set the IsSelected property for true.
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    public async Task SetStudentAsTrue(List<Student> students)
    {
        var studentList = await _studentRepository.GetAllAsync();
        await ClearSelection(studentList);
        SelectableEntity<Student> selectableSomeEntity = new SelectableEntity<Student>();
        foreach (var student in students)
        {

            selectableSomeEntity.SetSelectionAsTrue(student);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// saves the number of students' absences.
    /// </summary>
    /// <param name="students"></param>
    /// <returns></returns>
    public async Task SetAbsencesNumber(List<Student> students)
    {
        try
        {
            foreach (var student in students)
            {
                var savedStudent = await GetByIdAsync(student.Id);

                savedStudent.Absences = student.Absences;
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task DeleteStudent(Student student)
    {
        var savedStudent = await _studentRepository.GetByIdAsync(student.Id);
        _studentRepository.DeleteAsync(savedStudent);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddStudentAsync(Student student)
    {
        _studentRepository.Add(student);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<Student>> GetStudentsSelectedByClasses(IEnumerable<int> selectedClasses)
        => await _studentRepository.GetStudentsSelectedByClasses(selectedClasses);

    public async Task<List<Student>> GetByIsSelectedProperty()
        => await _studentRepository.GetByIsSelectedProperty();

    public async Task<List<Student>> GetStudentsByIdListAsync(List<int> ids)
        => await _studentRepository.GetStudentsByIdListAsync(ids);
}