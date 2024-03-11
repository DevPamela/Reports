namespace Reports.Domain.Models
{
    public class Goal
    {
        public string Name { get; set; }
        public bool IsSatisfactory { get; set; }

        public void IsAtended()
        {
            if (IsSatisfactory)
            {
                this.GetGoalsReachedFromDB();
            }
            else
            {
                this.GetGoalsNotReachedFromDB();
               
            }
        }
    }
}