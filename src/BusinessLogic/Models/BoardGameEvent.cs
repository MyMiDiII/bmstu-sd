using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class BoardGameEvent
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public uint Duration { get; set; }
        public uint Cost { get; set; }
        public bool Purchase { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}
