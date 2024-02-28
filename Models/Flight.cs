using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FlightNumber { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public TimeOnly DepartureTime { get; set; }

        public TimeOnly ArrivalTime { get; set; }

        public int FlightDurationMin { get; set; }

        public decimal Price { get; set; }

        public string Airline { get; set; }
    }
}
