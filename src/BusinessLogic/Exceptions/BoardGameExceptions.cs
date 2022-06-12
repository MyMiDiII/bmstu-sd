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
}
