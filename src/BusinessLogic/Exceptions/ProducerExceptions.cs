namespace BusinessLogic.Exceptions
{
    public class ProducerException : Exception
    {
        public ProducerException() : base() { }
        public ProducerException(string message) : base(message) { }
        public ProducerException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class AlreadyExistsProducerException : ProducerException { }
    public class NotExistsProducerException : ProducerException { }
    public class AddProducerException : ProducerException { }
    public class UpdateProducerException : ProducerException { }
    public class AlreadyDeletedProducerException : ProducerException { }
}
