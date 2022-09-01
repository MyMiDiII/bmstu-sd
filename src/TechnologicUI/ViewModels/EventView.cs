using Blazorise;
using BusinessLogic.Models;

namespace TechnologicUI.ViewModels
{
    public class EventView
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public DateOnly Date { get; set; }
        public string StartTimeView { get; set; }
        public string DurationView { get; set; }
        public uint Cost { get; set; }
        public string PurchaseView { get; set; }
        public Color StateColor { get; set; }
        public string StateName { get; set; }

        public EventView(BoardGameEvent bgEvent)
        {
            ID = bgEvent.ID;
            Title = bgEvent.Title;
            Date = bgEvent.Date;
            StartTimeView = bgEvent.StartTime.ToString("HH:mm");

            var duration = bgEvent.Duration;
            var hours = duration / 60;
            var minutes = duration % 60;
            DurationView = (hours != 0 && minutes != 0) ? $"{hours} ч. {minutes} мин."
                         : (hours != 0) ? $"{hours} ч."
                         : $"{minutes} мин.";

            Cost = bgEvent.Cost;
            PurchaseView = bgEvent.Purchase ? "Есть" : "Нет";

            var state = bgEvent.State;
            switch (state)
            {
                case BoardGameEventState.Planned:
                    StateColor = Color.Warning;
                    StateName = "Запланирована";
                    break;
                case BoardGameEventState.Registration:
                    StateColor = Color.Success;
                    StateName = "Регистрация";
                    break;
                case BoardGameEventState.Ready:
                    StateColor = Color.Info;
                    StateName = "Скоро начнется";
                    break;
                case BoardGameEventState.Started:
                    StateColor = Color.Secondary;
                    StateName = "Идет";
                    break;
                case BoardGameEventState.Finished:
                    StateColor = Color.Dark;
                    StateName = "Проведена";
                    break;
                case BoardGameEventState.Cancelled:
                    StateColor = Color.Danger;
                    StateName = "Отменена";
                    break;
                default:
                    break;
            }
        }
    }
}
