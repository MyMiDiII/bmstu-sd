using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class FavoriteBoardGame
    {
        [Key]
        [ForeignKey("BoardGameID")]
        public long BoardGameID { get; set; }
        [Key]
        [ForeignKey("PlayerID")]
        public long PlayerID { get; set; }

        public virtual BoardGame BoardGame { get; set; }
        public virtual Player Player { get; set; }

        public FavoriteBoardGame(long boardGameID, long playerID)
        {
            BoardGameID = boardGameID;
            PlayerID = playerID;
        }
    }
}
