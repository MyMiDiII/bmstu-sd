namespace BusinessLogic.Models
{
    public class RegisterRequest 
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public RegisterRequest(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
