using System.ComponentModel.DataAnnotations;

namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required]
        public string HotelName { get; set; }

        public string? Location { get; set; }

        public decimal Price { get; set; }

        public int Capacity { get; set; }

        public TimeOnly CheckInTime { get; set; }

        public TimeOnly CheckOutTime { get; set; }

        public string? Description { get; set; }
    }
}
