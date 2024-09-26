namespace Lab1.Models.Repositories
{
    public interface IStudentRepository
    {
        List<Student> GetStudents();
        void AddStudent(Student student);
    }
}

