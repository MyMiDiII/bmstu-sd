using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class BoardGame
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Producer { get; set; }
        public uint Year { get; set; }
        public uint MaxAge { get; set; }
        public uint MinAge { get; set; }
        public uint MaxPlayerNum { get; set; }
        public uint MinPlayerNum { get; set; }
        public uint MaxDuration { get; set; }
        public uint MinDuration { get; set; }
        public bool Deleted { get; set; } = false;

        public BoardGame(string title)
        {
            Title = title;
        }
    }
}
