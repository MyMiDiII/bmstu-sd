namespace BusinessLogic.Models
{
    public class User
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
            Roles = new List<Role>();
        }
    }
}
