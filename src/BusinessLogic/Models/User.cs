namespace BusinessLogic.Models
{
    public class User
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public long RoleID { get; set; }
    }
}
