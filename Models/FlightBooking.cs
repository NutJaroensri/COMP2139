using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class FlightBooking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FlightNumber { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string FlightStatus { get; set; }

        [Required]
        public DateTime DepartureDateTime { get; set; }

        [Required]
        public DateTime ArrivalDateTime { get; set; }
    }
}
