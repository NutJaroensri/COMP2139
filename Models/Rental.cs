using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PlateNumber { get; set; }

        public string RentalProvider { get; set; }

        public string ModelName { get; set; }

        public string ModelDescription { get; set; }

        public string ModelType { get; set; }

        public decimal Price { get; set; }

        public bool IsAvaiable { get; set;}

    }
}
