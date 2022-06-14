﻿using bcrypt = BCrypt.Net;

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
        public long GetCurrentUserID();
        public User GetCurrentUser();
        public void Login(LoginRequest loginRequest);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private User _curUser;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _curUser = new User
            {
                ID = 0,
                Name = "guest",
                Role = "guest"
            };
        }

        public long GetCurrentUserID()
        {
            return _curUser.ID;
        }

        public User GetCurrentUser()
        {
            return _curUser;
        }

        public void SetCurrentUser(User user)
        {
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
            var foundUser = _userRepository.GetByName(user.Name);
            return foundUser != null && user.Role == foundUser.Role;
        }

        private bool NotExist(long id)
        {
            return _userRepository.GetByID(id) == null;
        }

        private bool ValidatePassword(string textPassword, string hashPassword)
        {
            return bcrypt.BCrypt.Verify(textPassword, hashPassword);
        }

        public void Login(LoginRequest loginRequest)
        {
            var tmpUser = new User()
            {
                Name = loginRequest.Name,
                Password = loginRequest.Password,
                Role = loginRequest.Role
            };

            if (!Exist(tmpUser))
                throw new NotExistsUserException();

            var existingUser = _userRepository.GetByName(tmpUser.Name);

            if (!ValidatePassword(tmpUser.Password, existingUser.Password))
                throw new IncorrectUserPasswordException();

            if (!_userRepository.ConnectUserToDataStore(existingUser))
                throw new FailedConnectionToDataStoreException();

            SetCurrentUser(existingUser);
        }
    }
}
