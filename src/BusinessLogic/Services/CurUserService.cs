using BusinessLogic.Exceptions;
using BusinessLogic.IRepositories;
using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public class CurUserService
    {
        private User _curUser;

        public CurUserService()
        {
            _curUser = new User("guest", "guest")
            {
                CurRoleName = "guest"
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

        public long GetCurrentUserRoleID(string roleName)
        {
            var role = _curUser.Roles.Find(x => x.RoleName == roleName);
            return role == null ? -1 : role.RoleID;
        }

        public Role? GetCurrentUserRole(string roleName)
        {
            return _curUser.Roles.Find(x => x.RoleName == roleName);
        }

        public string GetCurrentUserRoleName()
        {
            return _curUser.CurRoleName;
        }

        public string? SetCurrentUserRole(string roleName)
        {
            var role = GetCurrentUserRole(roleName);

            if (role != null)
            {
                _curUser.CurRoleName = role.RoleName;
                return role.RoleName;
            }

            return null;
        }

        public void SetCurrentUser(User user, string roleName)
        {
            _curUser = user;
            user.CurRoleName = GetCurrentUserRole(roleName) == null
                               ? user.Roles[0].RoleName : roleName;
            _curUser = user;
        }

        public bool CheckUserRole(string role)
        {
            return _curUser.Roles.FirstOrDefault(item => item.RoleName == role) is not null;
        }
    }
}
