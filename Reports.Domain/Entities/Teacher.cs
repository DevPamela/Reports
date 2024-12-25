namespace Reports.Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }


        public User User { get; set; }
        public List<ClassRoom> Class { get; set; } = new();
        public List<Student> Students { get; set; } = new();
        public List<Goal> Goals { get; set; } = new();
    }
}