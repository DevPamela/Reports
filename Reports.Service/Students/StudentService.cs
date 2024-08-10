using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;
using Reports.Service.ExcelFileHeader;

namespace Reports.Service.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBulkInsertHelper _bulkInsertHelper;
        private readonly IExcelFileHeaderService _excelHeader;
        public StudentService(
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork,
            IBulkInsertHelper bulkInsertHelper, IExcelFileHeaderService excelHeader)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _bulkInsertHelper = bulkInsertHelper;
            _excelHeader = excelHeader;
        }
        /// <summary>
        /// Add a list of students.
        /// </summary>
        /// <param name="excelFile"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Student>> AddStudentByListAsync(IFormFile excelFile)
        {
            try
            {
                var students = await _excelHeader.HeadSheet(excelFile);

                //string[] columnNames = new[] { "Id", "Name", "Birthday", "Genre", "Absences", "TeacherId", "ClassroomID", "IsSelected" }; 

                //await _bulkInsertHelper.BulkInsert(students, "students", columnNames);
                //await _unitOfWork.SaveChangesAsync();
                return students.ToList();
            }
            catch 
            {
                throw new ArgumentException("Erro ao cadastrar turma");
            }
        }

        /// <summary>
        /// update the student list.
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        public async Task UpdateStudentList(List<Student> students)
        {

            foreach (var student in students)
            {
                var repo = await _studentRepository.GetByNameAsync(student.Name);
                repo.Genre = student.Genre;
                _studentRepository.Update(repo);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStudent(Student student)
        {
            try
            {
                var repo = await _studentRepository.GetByNameAsync(student.Name);
                repo.Genre = student.Genre;
                repo.Name = student.Name;
                repo.ClassRoomId = student.ClassRoomId;
                //Estourar uma exception se student.Birthday < DateTime.Now
                if (student.Birthday != DateTime.MinValue && student.Birthday < DateTime.Now)
                    repo.Birthday = student.Birthday;

                _studentRepository.Update(repo);

                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                throw new ArgumentException("Erro ao atualizar dados do aluno");
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
            catch 
            {
                throw new ArgumentException("Erro ao inserir as faltas do aluno");
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
    }
}