﻿using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IBoardGameEventRepository : IRepository<BoardGameEvent>
    {
        List<BoardGameEvent> GetByTitle(string title);
        List<BoardGameEvent> GetByDate(DateOnly date);
        List<BoardGameEvent> GetByStartTime(TimeOnly startTime);
        List<BoardGameEvent> GetByDuration(uint duration);
        List<BoardGameEvent> GetByCost(uint cost);
        List<BoardGameEvent> GetByRegistration(TimeOnly time);
        List<BoardGameEvent> GetByPurchase(bool purchase);
        List<BoardGame> GetEventGames(long evendID);
    }
}
