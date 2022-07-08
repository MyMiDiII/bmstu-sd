namespace BusinessLogic.Exceptions
{
    public class BoardGameException : Exception
    {
        public BoardGameException() : base() { }
        public BoardGameException(string message) : base(message) { }
        public BoardGameException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class AlreadyExistsBoardGameException : BoardGameException { }
    public class NotExistsBoardGameException : BoardGameException { }
    public class AddBoardGameException : BoardGameException { }
    public class UpdateBoardGameException : BoardGameException { }
    public class AlreadyDeletedBoardGameException : BoardGameException { }
    public class WrongMinMaxBoardGameException : BoardGameException { }
    public class AddEventGameException : BoardGameException { }
    public class DeleteEventGameException : BoardGameException { }
    public class AlreadyExistsEventGameException : BoardGameException { }
    public class AddFavoriteGameException : BoardGameException { }
    public class DeleteFavoriteGameException : BoardGameException { }
    public class AlreadyExistsFavoriteGameException : BoardGameException { }
}
