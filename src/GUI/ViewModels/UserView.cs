using BusinessLogic.Models;

namespace GUI.ViewModels
{
    public class UserView
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public bool IsPlayer{ get; set; }
        public bool IsOrganizer { get; set; }
        public bool IsAdmin{ get; set; }

        public UserView(User user)
        {
            ID = user.ID;
            Name = user.Name;
            IsPlayer = user.Roles.Any(r => r.RoleName == "player");
            IsOrganizer = user.Roles.Any(r => r.RoleName == "organizer");
            IsAdmin = user.Roles.Any(r => r.RoleName == "admin");
        }
    }
}
