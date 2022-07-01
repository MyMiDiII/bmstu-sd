using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class BoardGameEvent
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public uint Duration { get; set; }
        public uint Cost { get; set; }
        public bool Purchase { get; set; }
        [ForeignKey("Organizer")]
        public long OrganizerID { get; set; }
        public long VenueID { get; set; }

        public BoardGameEvent(string title, DateOnly date)
        {
            Title = title;
            Date = date;
        }
    }
}
