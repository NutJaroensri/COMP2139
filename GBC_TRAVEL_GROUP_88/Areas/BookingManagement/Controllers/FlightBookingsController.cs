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
    public class FlightBookingsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<FlightBookingsController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;
        public FlightBookingsController(AppDbContext db, ILogger<FlightBookingsController> logger, UserManager<ApplicationUser> userManager)
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
                var allBookings = await _db.FlightBookings.ToListAsync();
                return View(allBookings);
            }
            else
            {
                // Retrieve bookings associated with the current user
                var currentUserId = user.Id;
                var userBookings = await _db.FlightBookings.Where(b => b.UserId == currentUserId).ToListAsync();
                return View(userBookings);
            }
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var flightBooking = await _db.FlightBookings.FirstOrDefaultAsync(f => f.FlightBookingId == id);
            if (flightBooking == null)
            {
                return NotFound();
            }
            return View(flightBooking);
        }

        [HttpGet("Create")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var flights = await _db.Flights.ToListAsync();
            return View(flights);

        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(int flightId)
        {
            var flight = await _db.Flights.FindAsync(flightId);
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if user is not authenticated
            }

            // Create a new FlightBooking instance
            var flightBooking = new FlightBooking
            {
                UserId = user.Id,
                Username = user.UserName,
                FlightId = flight.FlightId,
                FlightNumber = flight.FlightNumber,
                Airline = flight.Airline,
                Origin = flight.Origin,
                Destination = flight.Destination,
                DepartureTime = flight.DepartureTime,   
                ArrivalTime = flight.ArrivalTime
            };


            if (ModelState.IsValid)
            {
                await _db.FlightBookings.AddAsync(flightBooking);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Flight Booking created successfully: {FlightNumber}, {Username}", flightBooking.FlightNumber, flightBooking.Username);
                return RedirectToAction("Index");
            }

            return View(flightBooking);
        }

        [Authorize]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var flightBooking = await _db.FlightBookings.FirstOrDefaultAsync(f => f.FlightBookingId == id);
            if (flightBooking == null)
            {
                return NotFound();
            }
            return View(flightBooking);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirm")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int FlightBookingId)
        {
            var flightBooking = await _db.FlightBookings.FindAsync(FlightBookingId);
            if (flightBooking != null)
            {
                _db.FlightBookings.Remove(flightBooking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();


        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var flightBookingsQuery = from f in _db.FlightBookings
                               select f;
            bool searchPerformed = !string.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                flightBookingsQuery = flightBookingsQuery.Where(f => f.Username.Contains(searchString)
                                                             || f.Airline.Contains(searchString)
                                                             || f.FlightNumber.Contains(searchString)
                                                             || f.Destination.Contains(searchString)
                                                             || f.Origin.Contains(searchString));
                                                             
            }

            var flightBookings = await flightBookingsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", flightBookings);

        }

    }
}
