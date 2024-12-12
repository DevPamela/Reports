namespace Reports.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Teacher Teacher { get; set; }
    }
}
