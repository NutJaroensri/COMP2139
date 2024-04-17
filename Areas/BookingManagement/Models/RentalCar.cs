using System.ComponentModel.DataAnnotations;

namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models
{
    public class RentalCar
    {
        [Key]
        public int RentalCarId { get; set; }

        [Required]
        public string PlateNumber { get; set; }

        public string RentalProvider { get; set; }

        public string ModelName { get; set; }

        public string? ModelDescription { get; set; }

        public string? ModelType { get; set; }

        public decimal Price { get; set; }

        public bool IsAvaiable { get; set; }
    }
}
