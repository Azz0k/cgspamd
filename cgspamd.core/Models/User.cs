namespace cgspamd.core.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Hash { get; set; }
        public required string FullName { get; set; }
        public bool Enabled { get; set; }
        public bool Deleted { get; set; }
        public int TokenVersion { get; set; }
        public bool IsAdmin { get; set; }
    }
}
