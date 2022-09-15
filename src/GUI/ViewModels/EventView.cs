using Blazorise;
using BusinessLogic.Models;

namespace GUI.ViewModels
{
    public class EventView
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public DateOnly Date { get; set; }
        public string StartTimeView { get; set; }
        public string DurationView { get; set; }
        public uint Cost { get; set; }
        public bool Purchase { get; set; }
        public string PurchaseView { get; set; }
        public Color StateColor { get; set; }
        public string StateName { get; set; }
        public Color RegisterColor { get; set; }
        public string RegisterName { get; set; }
        public Organizer? Organizer { get; set; }
        public Venue? Venue { get; set; }
        public DateTime BeginRegistration { get; set; }
        public DateTime EndRegistration { get; set; }

        public EventView(BoardGameEvent bgEvent, Organizer? organizer = null, Venue? venue = null)
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
            Purchase = bgEvent.Purchase;
            PurchaseView = bgEvent.Purchase ? "Есть" : "Нет";

            var state = bgEvent.State;
            switch (state)
            {
                case BoardGameEventState.Planned:
                    StateColor = Color.Warning;
                    StateName = "Запланирована";
                    RegisterColor = Color.Warning;
                    RegisterName = "Скоро начнется";
                    break;
                case BoardGameEventState.Registration:
                    StateColor = Color.Success;
                    StateName = "Регистрация";
                    RegisterColor = Color.Info;
                    RegisterName = "Идет";
                    break;
                case BoardGameEventState.Ready:
                    StateColor = Color.Info;
                    StateName = "Скоро начнется";
                    RegisterColor = Color.Danger;
                    RegisterName = "Закончена";
                    break;
                case BoardGameEventState.Started:
                    StateColor = Color.Secondary;
                    StateName = "Идет";
                    RegisterColor = Color.Danger;
                    RegisterName = "Закончена";
                    break;
                case BoardGameEventState.Finished:
                    StateColor = Color.Dark;
                    StateName = "Проведена";
                    RegisterColor = Color.Danger;
                    RegisterName = "Закончена";
                    break;
                case BoardGameEventState.Cancelled:
                    StateColor = Color.Danger;
                    StateName = "Отменена";
                    RegisterColor = Color.Dark;
                    RegisterName = "Закрыта";
                    break;
                default:
                    StateColor = Color.Danger;
                    StateName = "Удалена";
                    break;
            }

            Organizer = organizer;
            Venue = venue;

            BeginRegistration = bgEvent.BeginRegistration;
            EndRegistration = bgEvent.EndRegistration;
        }
    }
}
