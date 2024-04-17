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
    public class HotelBookingsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<HotelBookingsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HotelBookingsController(AppDbContext db, ILogger<HotelBookingsController> logger, UserManager<ApplicationUser> userManager)
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
                var allBookings = await _db.HotelBookings.ToListAsync();
                return View(allBookings);
            }
            else
            {
                // Retrieve bookings associated with the current user
                var currentUserId = user.Id;
                var userBookings = await _db.HotelBookings.Where(b => b.UserId == currentUserId).ToListAsync();
                return View(userBookings);
            }
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var hotelBooking = await _db.HotelBookings.FirstOrDefaultAsync(f => f.HotelBookingId == id);
            if (hotelBooking == null)
            {
                return NotFound();
            }
            return View(hotelBooking);
        }

        [HttpGet("Create")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var hotels = await _db.Hotels.ToListAsync();
            return View(hotels);

        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(int hotelId)
        {
            var hotel = await _db.Hotels.FindAsync(hotelId);
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if user is not authenticated
            }

            // Create a new HotelBooking instance
            var hotelBooking = new HotelBooking
            {
                UserId = user.Id,
                Username = user.UserName,
                HotelId = hotel.HotelId,
                HotelName = hotel.HotelName,
                Location = hotel.Location,
                CheckInTime = hotel.CheckInTime,
                CheckOutTime = hotel.CheckOutTime,
                Description = hotel.Description
            };


            if (ModelState.IsValid)
            {
                await _db.HotelBookings.AddAsync(hotelBooking);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Hotel Booking created successfully: {HotelName}, {Username}", hotelBooking.HotelName, hotelBooking.Username);
                return RedirectToAction("Index");
            }

            return View(hotelBooking);
        }


        [Authorize]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hotelBooking = await _db.HotelBookings.FirstOrDefaultAsync(h => h.HotelBookingId == id);
            if (hotelBooking == null)
            {
                return NotFound();
            }
            return View(hotelBooking);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirm")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int HotelBookingId)
        {
            var hotelBooking = await _db.HotelBookings.FindAsync(HotelBookingId);
            if (hotelBooking != null)
            {
                _db.HotelBookings.Remove(hotelBooking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();


        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var hotelBookingsQuery = from h in _db.HotelBookings
                                      select h;
            bool searchPerformed = !string.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                hotelBookingsQuery = hotelBookingsQuery.Where(h => h.Username.Contains(searchString)
                                                             || h.HotelName.Contains(searchString)
                                                             || h.Location.Contains(searchString));

            }

            var hotelBookings = await hotelBookingsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", hotelBookings);

        }
    }
}
