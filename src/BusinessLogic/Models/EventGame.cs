namespace BusinessLogic.Models
{
    public class EventGame
    {
        public long ID { get; set; }
        public long BoardGameEventID { get; set; }
        public long BoardGameID { get; set; }

        public EventGame(long boardGameEventID, long boardGameID)
        {
            BoardGameEventID = boardGameEventID;
            BoardGameID = boardGameID;
        }
    }
}
