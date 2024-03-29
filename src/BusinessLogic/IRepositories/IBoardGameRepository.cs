﻿using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IBoardGameRepository : IRepository<BoardGame>
    {
        List<BoardGame> GetByTitle(string title); 
        List<BoardGame> GetByProducer(string producer);
        List<BoardGame> GetByYear(uint year);
        List<BoardGame> GetByAge(uint minAge, uint maxAge);
        List<BoardGame> GetByPlayerNum(uint minNum, uint maxNum);
        List<BoardGame> GetByDuration(uint minDuration, uint maxDuration);
        void AddToEvent(long gameID, long eventID);
        void AddManyToEvent(List<long> gamesIDs, long eventID);
        void DeleteFromEvent(long gameID, long eventID);
        bool CheckGamePlaying(long gameID, long eventID);
        List<BoardGameEvent> GetGameEvents(long gameID);
        void AddToFavorites(long gameID, long playerID);
        void DeleteFromFavorites(long gameID, long playerID);
        bool CheckGameInFavorites(long gameID, long playerID);
    }
}
