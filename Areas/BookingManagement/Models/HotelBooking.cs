using System.ComponentModel.DataAnnotations;

namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models
{
    public class HotelBooking
    {
        [Key]
        public int HotelBookingId { get; set; }

        public string? UserId { get; set; }
        public string? Username { get; set; }

        public int? HotelId { get; set; }

        public string? HotelName { get; set; }

        public string? Location { get; set; }

        public TimeOnly? CheckInTime { get; set; }

        public TimeOnly? CheckOutTime { get; set; }

        public string? Description { get; set; }
    }
}
