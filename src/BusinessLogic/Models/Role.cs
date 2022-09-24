using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class Role
    {
        [Key]
        public long ID { get; set; }
        public string RoleName { get; set; }
        public long RoleID { get; set; }
        [ForeignKey("UserID")]
        public long UserID { get; set; }
        public virtual User User { get; set; }

        public Role(string roleName) { RoleName = roleName; }
    }
}
