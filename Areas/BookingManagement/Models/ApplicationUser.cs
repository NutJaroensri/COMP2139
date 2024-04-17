using Microsoft.AspNetCore.Identity;
namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models
{
    public class ApplicationUser : IdentityUser
    {

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int UserNameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture {  get; set; }
        public string? ContactInformation { get; set; }
        public string? Preferences { get; set; }
    }
}
