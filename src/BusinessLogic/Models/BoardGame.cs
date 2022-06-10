using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class BoardGame
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Produser { get; set; }
        public uint Year { get; set; }
        public uint MaxAge { get; set; }
        public uint MinAge { get; set; }
        public uint MaxPlayerNum { get; set; }
        public uint MinPlayerNum { get; set; }
        public uint MaxDuration { get; set; }
        public uint MinDuration { get; set; }
    }
}
