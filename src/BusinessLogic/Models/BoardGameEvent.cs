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
        public TimeOnly RegistrationTime { get; set; }
        public BoardGameEventState State { get; set; }
        [ForeignKey("Organizer")]
        public long OrganizerID { get; set; }
        public long VenueID { get; set; }
        public bool Deleted { get; set; }

        public BoardGameEvent(string title, DateOnly date)
        {
            Title = title;
            Date = date;
            State = BoardGameEventState.Planned;
            Deleted = false;
        }
    }
}
