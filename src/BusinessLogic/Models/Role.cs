﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class Role
    {
        [Key]
        public long ID { get; set; }
        public string RoleName { get; set; }
        public long RoleID { get; set; }
        [ForeignKey("User")]
        public long UserID { get; set; }

        public Role(string roleName) { RoleName = roleName; }
    }
}
