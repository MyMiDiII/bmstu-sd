using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class EventGame
    {
        [Key]
        public long ID { get; set; }
        [ForeignKey("BoardGameEvent")]
        public long BoardGameEventID { get; set; }
        [ForeignKey("BoardGame")]
        public long BoardGameID { get; set; }

        public EventGame(long boardGameEventID, long boardGameID)
        {
            BoardGameEventID = boardGameEventID;
            BoardGameID = boardGameID;
        }
    }
}
