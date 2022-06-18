using BusinessLogic.Exceptions;
using BusinessLogic.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Services;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        List<User> GetUsers();
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        public long GetCurrentUserID();
        public User GetCurrentUser();
        public long GetCurrentUserRoleID(string role);
        public void Login(LoginRequest loginRequest);
        public void Register(RegisterRequest registerRequest);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private User _curUser;
        private readonly IEncryptionService _encryptionService;

        public UserService(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _curUser = _userRepository.GetDefauldUser();
            _encryptionService = encryptionService;
        }

        public long GetCurrentUserID()
        {
            return _curUser.ID;
        }

        public User GetCurrentUser()
        {
            return _curUser;
        }

        public long GetCurrentUserRoleID(string roleName)
        {
            var role = _curUser.Roles.Find(x => x.RoleName == roleName);
            return role == null ? -1 : role.RoleID;
        }

        private void SetCurrentUser(User user)
        {
            user.Roles = _userRepository.GetUserRoles(user.ID);
            _curUser = user;
        }

        public List<User> GetUsers()
        {
            return _userRepository.GetAll();
        }

        public void CreateUser(User user)
        {
            if (Exist(user))
                throw new AlreadyExistsUserException();

            _userRepository.Add(user);
        }

        public void UpdateUser(User user)
        {
            if (NotExist(user.ID))
                throw new NotExistsUserException();

            _userRepository.Update(user);
        }

        public void DeleteUser(User user)
        {
            if (NotExist(user.ID))
                throw new NotExistsUserException();

            _userRepository.Delete(user);
        }

        private bool Exist(User user)
        {
            return _userRepository.GetByName(user.Name) != null;
        }

        private bool NotExist(long id)
        {
            return _userRepository.GetByID(id) == null;
        }

        public void Login(LoginRequest loginRequest)
        {
            var tmpUser = new User()
            {
                Name = loginRequest.Name,
                Password = loginRequest.Password
            };

            var existingUser = _userRepository.GetByName(tmpUser.Name);

            if (existingUser == null)
                throw new NotExistsUserException();

            if (!_encryptionService.ValidatePassword(tmpUser.Password, existingUser.Password))
                throw new IncorrectUserPasswordException();

            if (!_userRepository.ConnectUserToDataStore(existingUser))
                throw new FailedConnectionToDataStoreException();

            SetCurrentUser(existingUser);
        }

        public void Register(RegisterRequest registerRequest)
        {
            var tmpUser = new User()
            {
                Name = registerRequest.Name,
                Password = registerRequest.Password
            };

            if (_userRepository.GetByName(tmpUser.Name) != null)
                throw new AlreadyExistsUserException();

            tmpUser.Password = _encryptionService.HashPassword(tmpUser.Password);

            _userRepository.AddWithBasicRole(tmpUser);
        }
    }
}
