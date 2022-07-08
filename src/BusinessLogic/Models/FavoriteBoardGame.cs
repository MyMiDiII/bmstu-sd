using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class FavoriteBoardGame
    {
        [Key]
        [ForeignKey("BoardGame")]
        public long BoardGameID { get; set; }
        [Key]
        [ForeignKey("Player")]
        public long PlayerID { get; set; }

        public FavoriteBoardGame(long boardGameID, long playerID)
        {
            BoardGameID = boardGameID;
            PlayerID = playerID;
        }
    }
}
