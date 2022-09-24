namespace BusinessLogic.Exceptions
{
    public class VenueException : Exception
    {
        public VenueException() : base() { }
        public VenueException(string message) : base(message) { }
        public VenueException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class AlreadyExistsVenueException : VenueException { }
    public class NotExistsVenueException : VenueException { }
    public class AddVenueException : VenueException { }
    public class UpdateVenueException : VenueException { }
    public class AlreadyDeletedVenueException : VenueException { }
}
