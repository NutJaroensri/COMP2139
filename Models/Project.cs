using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Project
    {
        public int Project_id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
