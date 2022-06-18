namespace BusinessLogic.Models
{
    public class Venue
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public string? URL { get; set; }
        public string? PhoneNumber { get; set; }

        public Venue(string name, string type, string address)
        {
            Name = name;
            Type = type;
            Address = address;
            Email = null;
            URL = null;
            PhoneNumber = null;
        }
    }
}
