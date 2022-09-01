using BusinessLogic.Models;

namespace TechnologicUI.ViewModels
{
    public class BoardGameView
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string? Producer { get; set; }
        public uint Year { get; set; }
        public string AgeView { get; set; }
        public string PlayersView { get; set; }
        public string DurationView { get; set; }

        public BoardGameView(BoardGame game)
        {
            ID = game.ID;
            Title = game.Title;
            Producer = game.Producer;
            Year = game.Year;
            AgeView = game.MaxAge != 150 ? $"{game.MinAge}-{game.MaxAge}" : $"{game.MinAge}+";
            PlayersView = game.MaxPlayerNum == game.MinPlayerNum
                          ? $"{game.MaxPlayerNum}"
                          : game.MaxPlayerNum == 100
                          ? $"{game.MinPlayerNum}+"
                          : $"{game.MinPlayerNum}-{game.MaxPlayerNum}";
            DurationView = game.MaxDuration == game.MinDuration
                           ? $"{game.MaxDuration}"
                           : game.MaxDuration == 1440
                           ? $"{game?.MinDuration}+"
                           : $"{game?.MinDuration}-{game?.MaxDuration}";
        }
    }
}
