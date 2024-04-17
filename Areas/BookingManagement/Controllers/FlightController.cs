using GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Controllers;
using GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models;
using GBC_TRAVEL_GROUP_88.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Controllers
{
    [Area("BookingManagement")]
    [Route("[area]/[controller]/[action]")]
    public class FlightsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<FlightsController> _logger;
        public FlightsController(AppDbContext db, ILogger<FlightsController> logger)
        {
            _db = db;
            _logger = logger;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var flights = await _db.Flights.ToListAsync();
            return View(flights);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var flight = await _db.Flights.FirstOrDefaultAsync(f => f.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpGet("Create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Flight flight)
        {
            if (ModelState.IsValid)
            {
                await _db.Flights.AddAsync(flight);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Flight created successfully: {FlightID}, {FlightNumber}", flight.FlightId, flight.FlightNumber);
                return RedirectToAction("Index");
            }

            return View(flight);
        }

        [HttpGet("Edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var flight = await _db.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpPost("Edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId, FlightNumber, Airline, Origin, Destination, DepartureTime, ArrivalTime, FlightDurationMin, Price")] Flight flight)
        {
            _logger.LogInformation($"Received ID: {id}, FlightId: {flight.FlightId}");
            if (id != flight.FlightId)
            {

                //_logger.LogWarning("ID does not match ProjectId");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(flight); // no await is required here
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FlightExists(flight.FlightId))
                    {
                        return NotFound();

                    }
                    else
                    {
                        throw;
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        private async Task<bool> FlightExists(int id)
        {
            return await _db.Flights.AnyAsync(e => e.FlightId == id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var flight = await _db.Flights.FirstOrDefaultAsync(f => f.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirm")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int FlightId)
        {
            var flight = await _db.Flights.FindAsync(FlightId);
            if (flight != null)
            {
                _db.Flights.Remove(flight);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();


        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var flightsQuery = from f in _db.Flights
                               select f;
            bool searchPerformed = !string.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                flightsQuery = flightsQuery.Where(f => f.FlightNumber.Contains(searchString)
                                                             || f.Airline.Contains(searchString)
                                                             || f.Origin.Contains(searchString)
                                                             || f.Destination.Contains(searchString));
            }

            var flights = await flightsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", flights);

        }
    }
}
