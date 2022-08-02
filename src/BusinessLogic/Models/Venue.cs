using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Venue
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Address { get; set; }
        public string? Email { get; set; }
        public string? URL { get; set; }
        public string? PhoneNumber { get; set; }
        public bool Deleted { get; set; }

        public Venue(string name, string type, string address)
        {
            Name = name;
            Type = type;
            Address = address;
            Email = null;
            URL = null;
            PhoneNumber = null;
            Deleted = false;
        }
    }
}
