namespace BusinessLogic.Models
{
    public class LoginRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public LoginRequest(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
