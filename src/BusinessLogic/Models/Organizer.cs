using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class Organizer
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public string? URL { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
