namespace BusinessLogic.Models
{
    public class User
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; }
    }
}
