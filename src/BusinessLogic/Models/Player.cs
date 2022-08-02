using BusinessLogic.Config;

namespace BusinessLogic.Models
{
    public class Player
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string League { get; set; }
        public uint Rating { get; set; }
        public bool Deleted { get; set; }

        public Player(string name)
        {
            Name = name;
            League = PlayerConfig.Leagues.First();
            Rating = 0;
            Deleted = false;
        }
    }
}
