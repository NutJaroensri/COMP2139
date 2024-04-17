using GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models;
using GBC_TRAVEL_GROUP_88.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Controllers
{
	[Area("BookingManagement")]
	[Route("[area]/[controller]/[action]")]
	public class RentalCarBookingsController : Controller
	{
		private readonly AppDbContext _db;
		private readonly ILogger<RentalCarBookingsController> _logger;

		private readonly UserManager<ApplicationUser> _userManager;

		public RentalCarBookingsController(AppDbContext db, ILogger<RentalCarBookingsController> logger, UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
			_db = db;
			_logger = logger;
		}
		[Authorize]
		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return RedirectToAction("Login", "Account"); // Redirect to login if user is not authenticated
			}

			// Check if the user has the specified role
			var userHasRole = await _userManager.IsInRoleAsync(user, "7cb6c9fc-d9c4-4e59-be1d-156e6ef431ba");

			if (userHasRole)
			{
				// Retrieve all bookings since the user has the specified role
				var allBookings = await _db.RentalCarBookings.ToListAsync();
				return View(allBookings);
			}
			else
			{
				// Retrieve bookings associated with the current user
				var currentUserId = user.Id;
				var userBookings = await _db.RentalCarBookings.Where(b => b.UserId == currentUserId).ToListAsync();
				return View(userBookings);
			}

		}

		[HttpGet("Details/{id:int}")]
		public async Task<IActionResult> Details(int id)
		{
			var rentalCarBooking = await _db.RentalCarBookings.FirstOrDefaultAsync(f => f.RentalCarBookingId == id);
			if (rentalCarBooking == null)
			{
				return NotFound();
			}
			return View(rentalCarBooking);
		}

        [HttpGet("Create")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var rentalCars = await _db.RentalCars.ToListAsync();
            return View(rentalCars);

        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(int Id)
        {
            var rentalCar = await _db.RentalCars.FindAsync(Id);
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if user is not authenticated
            }

			// Create a new HotelBooking instance
			var rentalCarBooking = new RentalCarBooking
			{
				UserId = user.Id,
				Username = user.UserName,
				RentalCarId = rentalCar.RentalCarId,
				PlateNumber = rentalCar.PlateNumber,
				RentalProvider = rentalCar.RentalProvider,
				ModelName = rentalCar.ModelName,
				ModelDescription = rentalCar.ModelDescription,
				ModelType = rentalCar.ModelType,
				IsAvaiable = rentalCar.IsAvaiable
			
            };


            if (ModelState.IsValid)
            {
                await _db.RentalCarBookings.AddAsync(rentalCarBooking);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Hotel Booking created successfully");
                return RedirectToAction("Index");
            }

            return View(rentalCarBooking);
        }

        [Authorize]
		[HttpGet("Delete/{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			var rentalCarBooking = await _db.RentalCarBookings.FirstOrDefaultAsync(r => r.RentalCarBookingId == id);
			if (rentalCarBooking == null)
			{
				return NotFound();
			}
			return View(rentalCarBooking);
		}

		[HttpPost("DeleteConfirmed/{id:int}")]
		[HttpPost, ActionName("DeleteConfirm")]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int rentalCarBookingId)
		{
			var rentalCarBooking = await _db.RentalCarBookings.FindAsync(rentalCarBookingId);
			if (rentalCarBooking != null)
			{
				_db.RentalCarBookings.Remove(rentalCarBooking);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return NotFound();


		}

		[HttpGet("Search/{searchString?}")]
		public async Task<IActionResult> Search(string searchString)
		{
			var rentalCarBookingsQuery = from r in _db.RentalCarBookings
									 select r;
			bool searchPerformed = !string.IsNullOrEmpty(searchString);
			if (searchPerformed)
			{
				rentalCarBookingsQuery = rentalCarBookingsQuery.Where(r => r.Username.Contains(searchString)
															 || r.ModelName.Contains(searchString)
															 || r.RentalProvider.Contains(searchString));

			}

			var rentalCarBookings = await rentalCarBookingsQuery.ToListAsync();
			ViewData["SearchPerformed"] = searchPerformed;
			ViewData["SearchString"] = searchString;
			return View("Index", rentalCarBookings);

		}
	}
}
