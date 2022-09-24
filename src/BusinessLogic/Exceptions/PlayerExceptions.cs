namespace BusinessLogic.Exceptions
{
    public class PlayerException : Exception
    {
        public PlayerException() : base() { }
        public PlayerException(string message) : base(message) { }
        public PlayerException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class AlreadyExistsPlayerException : PlayerException { }
    public class NotExistsPlayerException : PlayerException { }
    public class UserIsNotPlayerException : PlayerException { }
    public class AlreadyExistsPlayerRegistraionException : PlayerException { }
    public class AddPlayerRegistrationException: PlayerException { }
    public class NotExistsPlayerRegistraionException : PlayerException { }
    public class AddPlayerException : PlayerException { }
    public class UpdatePlayerException : PlayerException { }
    public class AlreadyDeletedPlayerException : PlayerException { }
}
