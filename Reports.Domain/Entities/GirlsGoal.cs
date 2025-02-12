namespace Reports.Domain.Entities
{

    public class GirlsGoal : Goal
        {
            public string GirlsAtendedDescription { get; set; } = string.Empty;
            public string GirlsParciallyAtendedDescription { get; set; } = string.Empty;
            public string GirlsUnatendedDescription { get; set; } = string.Empty;
        }
    }