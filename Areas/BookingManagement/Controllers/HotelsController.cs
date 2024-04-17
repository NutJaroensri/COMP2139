using GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models;
using GBC_TRAVEL_GROUP_88.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Controllers
{
    [Area("BookingManagement")]
    [Route("[area]/[controller]/[action]")]
    public class HotelsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(AppDbContext db, ILogger<HotelsController> logger)
        {
            _db = db;
            _logger = logger;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var hotels = await _db.Hotels.ToListAsync();
            return View(hotels);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
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
        public async Task<IActionResult> Create(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                await _db.Hotels.AddAsync(hotel);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Hotel created successfully: {HotelID}, {HotelName}", hotel.HotelId, hotel.HotelName);
                return RedirectToAction("Index");
            }

            return View(hotel);
        }

        [HttpGet("Edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await _db.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost("Edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelId, HotelName, Location, Price, Capacity,CheckInTime, CheckOutTime, Description")] Hotel hotel)
        {
            _logger.LogInformation($"Received ID: {id}, HotelId: {hotel.HotelId}");
            if (id != hotel.HotelId)
            {

                //_logger.LogWarning("ID does not match HotelId");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(hotel); // no await is required here
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await HotelExists(hotel.HotelId))
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
            return View(hotel);
        }
        private async Task<bool> HotelExists(int id)
        {
            return await _db.Hotels.AnyAsync(e => e.HotelId == id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirm")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int HotelId)
        {
            var hotel = await _db.Hotels.FindAsync(HotelId);
            if (hotel != null)
            {
                _db.Hotels.Remove(hotel);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();


        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var hotelsQuery = from h in _db.Hotels
                               select h;
            bool searchPerformed = !string.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                hotelsQuery = hotelsQuery.Where(h => h.HotelName.Contains(searchString)
                                                             || h.Location.Contains(searchString));
            }

            var hotels = await hotelsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", hotels);

        }
    }
}
