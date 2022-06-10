using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class Player
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public uint Rating { get; set; }
        public string League { get; set; }
    }
}
