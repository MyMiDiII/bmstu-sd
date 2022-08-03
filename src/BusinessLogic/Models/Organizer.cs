using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Organizer
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string? Email { get; set; }
        public string? URL { get; set; }
        public string? PhoneNumber { get; set; }
        public bool Deleted { get; set; } = false;

        public Organizer(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
