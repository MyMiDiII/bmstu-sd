namespace BusinessLogic.Models
{
    public class BoardGame
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string? Produser { get; set; }
        public uint Year { get; set; }
        public uint MaxAge { get; set; }
        public uint MinAge { get; set; }
        public uint MaxPlayerNum { get; set; }
        public uint MinPlayerNum { get; set; }
        public uint MaxDuration { get; set; }
        public uint MinDuration { get; set; }
        public bool Deleted { get; set; }
        public List<BoardGameEvent> Events { get; set; } = new List<BoardGameEvent>();

        public BoardGame(string title) { Title = title; Deleted = false; }
    }
}
