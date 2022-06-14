namespace BusinessLogic.Exceptions
{
    public class UserException : Exception
    {
        public UserException() : base() { }
        public UserException(string message) : base(message) { }
        public UserException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class AlreadyExistsUserException : UserException { }
    public class NotExistsUserException : UserException { }
    public class IncorrectUserPasswordException : UserException { }
    public class FailedConnectionToDataStoreException : UserException { }
}
