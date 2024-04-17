using GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models;

namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models
{
    public class FlightBooking
    {
        public int FlightBookingId { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public int? FlightId { get; set; }
        public string? FlightNumber { get; set; }
        public string? Airline {  get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }
    }
}
