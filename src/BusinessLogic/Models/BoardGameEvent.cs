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
        public long OrganizerID { get; set; }
        public long VenueID { get; set; }
    }
}
