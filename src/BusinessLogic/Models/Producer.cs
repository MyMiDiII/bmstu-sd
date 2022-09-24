using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Producer
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string URL { get; set; } = "";
        public long Rating { get; set; } = 0;
        public bool Deleted { get; set; } = false;

        public Producer(string name)
        {
            Name = name;
        }
    }
}
