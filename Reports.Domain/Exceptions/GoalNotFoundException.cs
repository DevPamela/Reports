namespace Reports.Domain.Exceptions
{
    [Serializable]
    public class GoalNotFoundException : Exception
    {
        public GoalNotFoundException(string message)
        : base(message)
        {}
    }
}
