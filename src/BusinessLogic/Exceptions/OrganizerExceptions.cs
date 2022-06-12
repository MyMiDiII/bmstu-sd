namespace BusinessLogic.Exceptions
{
    public class OrganizerException : Exception
    {
        public OrganizerException() : base() { }
        public OrganizerException(string message) : base(message) { }
        public OrganizerException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class AlreadyExistsOrganizerException : OrganizerException { }
    public class NotExistsOrganizerException : OrganizerException { }
}
