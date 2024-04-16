namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models
{
    public class RentalCarBooking
    {
        public int RentalCarBookingId { get; set; }

        public string? UserId { get; set; }
        public string? Username { get; set; }

        public int? RentalCarId { get; set; }

        public string? PlateNumber { get; set; }

        public string? RentalProvider { get; set; }

        public string? ModelName { get; set; }

        public string? ModelDescription { get; set; }

        public string? ModelType { get; set; }

        public bool? IsAvaiable { get; set; }
    }
}
