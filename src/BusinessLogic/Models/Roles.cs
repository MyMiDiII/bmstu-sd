namespace BusinessLogic.Models
{
    public class Role
    {
        public string RoleName { get; set; }
        public long RoleID { get; set; }

        public Role(string roleName) { RoleName = roleName; }
    }
}
