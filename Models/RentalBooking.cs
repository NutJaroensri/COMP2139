using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class RentalBooking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? PlateNumber { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }
    }
}
