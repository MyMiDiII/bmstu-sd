using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public enum BoardGameEventState
    {
        Planned,
        Registration,
        Ready,
        Started,
        Finished,
        Cancelled,
        Deleted
    }

    public class BoardGameEvent
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public uint Duration { get; set; }
        public uint Cost { get; set; }
        public bool Purchase { get; set; }
        public DateTime BeginRegistration { get; set; }
        public DateTime EndRegistration { get; set; }
        public bool Cancelled { get; set; } = false;
        [ForeignKey("OrganizerID")]
        public long OrganizerID { get; set; }
        [ForeignKey("VenueID")]
        public long VenueID { get; set; }
        public bool Deleted { get; set; } = false;
        public BoardGameEventState State { get; set; }

        public virtual Organizer Organizer { get; set; }
        public virtual Venue Venue { get; set; }

        public BoardGameEvent()
        {
            Title = "";
            Date = new DateOnly();
        }

        public BoardGameEvent(string title, DateOnly date)
        {
            Title = title;
            Date = date;
            State = BoardGameEventState.Planned;
        }
    }
}
