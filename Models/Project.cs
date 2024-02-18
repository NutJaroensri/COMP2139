using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Project
    {
        public int Project_id { get; set; }
        [Required]
        [MaxLength(10)]
        [DisplayName("Project Name:")]
        public string Name { get; set; }

        //nullable ?
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        //nullable ?
        public string? Status { get; set; }

        public List<ProjectTask>? Tasks { get; set; }
    }
}
