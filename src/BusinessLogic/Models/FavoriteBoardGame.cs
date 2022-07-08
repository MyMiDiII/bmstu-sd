using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLogic.Models
{
    public class FavoriteBoardGame
    {
        [Key]
        [ForeignKey("Player")]
        public long PlayerID { get; set; }
        [Key]
        [ForeignKey("BoardGame")]
        public long BoardGameID { get; set; }
    }
}
