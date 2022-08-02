namespace BusinessLogic.Exceptions
{
    public class BoardGameEventException : Exception
    {
        public BoardGameEventException() : base() { }
        public BoardGameEventException(string message) : base(message) { }
        public BoardGameEventException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class AlreadyExistsBoardGameEventException : BoardGameEventException { }
    public class NotExistsBoardGameEventException : BoardGameEventException { }
    public class AddBoardGameEventException : BoardGameEventException { }
    public class UpdateBoardGameEventException : BoardGameEventException { }
    public class AlreadyDeletedBoardGameEventException : BoardGameEventException { }
}
