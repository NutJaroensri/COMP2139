using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? HotelName { get; set; }

        public string? Location { get; set; }

        public decimal Price { get; set; }

        public int Capacity { get; set; }

        public TimeOnly CheckInTime { get; set; }

        public TimeOnly CheckOutTime { get; set; }

        public string? Description { get; set; }
    }
}
