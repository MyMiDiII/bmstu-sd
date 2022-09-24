using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class EventGame
    {
        [Key]
        [ForeignKey("BoardGameID")]
        public long BoardGameID { get; set; }
        [Key]
        [ForeignKey("BoardGameEventID")]
        public long BoardGameEventID { get; set; }

        public virtual BoardGame BoardGame { get; set; }
        public virtual BoardGameEvent BoardGameEvent { get; set; }

        public EventGame(long boardGameID, long boardGameEventID)
        {
            BoardGameID = boardGameID;
            BoardGameEventID = boardGameEventID;
        }
    }
}
