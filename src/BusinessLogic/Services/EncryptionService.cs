using bcrypt = BCrypt.Net;

namespace BusinessLogic.Services
{
    public interface IEncryptionService
    {
        bool ValidatePassword(string textPassword, string hashPassword);
        string HashPassword(string textPassword);
    }

    public class BCryptEntryptionService: IEncryptionService
    {
        public bool ValidatePassword(string textPassword, string hashPassword)
        {
            return bcrypt.BCrypt.Verify(textPassword, hashPassword);
        }

        public string HashPassword(string textPassword)
        {
            return bcrypt.BCrypt.HashPassword(textPassword);
        }
    }
}
