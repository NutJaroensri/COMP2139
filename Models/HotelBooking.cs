using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class HotelBooking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }


        [Required]
        public string? NumberOfGuests { get; set; }

        public DateTime CheckInDateTime { get; set; }
        public DateTime CheckOutDateTime { get; set; }
    }
}
