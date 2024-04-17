using Microsoft.AspNetCore.Identity;
using GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models;
namespace GBC_TRAVEL_GROUP_88.Data
{
    public class ContextSeed
    {

        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.RoyalTraveler.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Traveler.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var superUser = new ApplicationUser
            {
                UserName = "Admin",
                Email = "adminsupport@domain.ca",
                FirstName = "Admin",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };


            // Check if the super user does not already exist in the database.
            if (userManager.Users.All(u => u.Id != superUser.Id))
            {
                // Attempt to find the super user by their email address.
                var user = await userManager.FindByEmailAsync(superUser.Email);

                // If the super user does not exist, proceed with creation.
                if (user == null)
                {
                    // Create the super user account with a specified password.
                    await userManager.CreateAsync(superUser, "P@ssword12$");
                    

                    // Assign the super user with all the following roles
                    await userManager.AddToRoleAsync(superUser, Enum.Roles.Traveler.ToString());
                    await userManager.AddToRoleAsync(superUser, Enum.Roles.RoyalTraveler.ToString());
                    await userManager.AddToRoleAsync(superUser, Enum.Roles.Admin.ToString());
                    
                }
            }
        }


    }
}
