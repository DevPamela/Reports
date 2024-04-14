namespace Reports.Domain.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AtendedDescription { get; set; } = string.Empty;
        public string ParciallyAtendedDescription { get; set; } = string.Empty;
        public string UnatendedDescription { get; set; } = string.Empty;
        //TODO: check nullability of foreign key
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public List<Student> Students { get; set; } = new();
        public List<Assessment> Assessments { get; set; } = new();

        //public void IsAtended()
        //{
        //    if (IsSatisfactory == "S")
        //    {
        //        this.GetGoalsReachedFromDB();
        //    }
        //    else if (IsSatisfactory == "PS")
        //    {
        //        this.GetGoalsPartiallyReachedFromDB();

        //    }
        //    else
        //    {
        //        this.GetGoalsNotReachedFromDB();
        //    }
        //}
    }
}