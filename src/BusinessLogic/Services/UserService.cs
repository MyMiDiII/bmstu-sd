using BusinessLogic.Exceptions;
using BusinessLogic.IRepositories;
using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        List<User> GetUsers();
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        long GetCurrentUserID();
        User GetCurrentUser();
        string? SetCurrentUserRole(string roleName);
        string GetCurrentUserRoleName();
        User? GetUserByID(long id);
        User? GetUserByName(string name);
        long GetCurrentUserRoleID(string role);
        bool CheckUserRole(string role);
        void Login(LoginRequest loginRequest);
        void Register(RegisterRequest registerRequest);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly CurUserService _curUserService;
        private readonly IEncryptionService _encryptionService;

        public UserService(IUserRepository userRepository, CurUserService curUserService,
                           IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _curUserService = curUserService;
            _encryptionService = encryptionService;
        }

        public long GetCurrentUserID()
        {
            return _curUserService.GetCurrentUserID();
        }

        public User GetCurrentUser()
        {
            return _curUserService.GetCurrentUser();
        }

        public User? GetUserByID(long id)
        {
            return _userRepository.GetByID(id);
        }

        public User? GetUserByName(string name)
        {
            return _userRepository.GetByName(name);
        }

        public long GetCurrentUserRoleID(string roleName)
        {
            var role = _curUserService.GetCurrentUser().Roles.Find(x => x.RoleName == roleName);
            return role == null ? -1 : role.RoleID;
        }

        public string GetCurrentUserRoleName()
        {
            return _curUserService.GetCurrentUserRoleName();
        }

        public string? SetCurrentUserRole(string roleName)
        {
            return _curUserService.SetCurrentUserRole(roleName);
        }

        public void SetCurrentUser(User user, string roleName)
        {
            user.Roles = _userRepository.GetUserRoles(user.ID);
            _curUserService.SetCurrentUser(user, roleName);
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

        public bool CheckUserRole(string role)
        {
            return _curUserService.CheckUserRole(role);
        }

        public void Login(LoginRequest loginRequest)
        {
            var tmpUser = new User(loginRequest.Name, loginRequest.Password);

            var existingUser = _userRepository.GetByName(tmpUser.Name);

            if (existingUser == null)
                throw new NotExistsUserException();

            if (!_encryptionService.ValidatePassword(tmpUser.Password, existingUser.Password))
                throw new IncorrectUserPasswordException();

            SetCurrentUser(existingUser, "player");
        }

        public void Register(RegisterRequest registerRequest)
        {
            var tmpUser = new User(registerRequest.Name, registerRequest.Password);

            if (_userRepository.GetByName(tmpUser.Name) != null)
                throw new AlreadyExistsUserException();

            tmpUser.Password = _encryptionService.HashPassword(tmpUser.Password);

            _userRepository.AddWithBasicRole(tmpUser);
        }
    }
}
