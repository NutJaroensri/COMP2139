using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        [Required]
        
        [DisplayName("Project Name")]
        public string Name { get; set; }

        // nullable ?
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // nullable ?
        public string? Status { get; set; }

        public List<ProjectTask>? Tasks { get; set; }
    }
}
