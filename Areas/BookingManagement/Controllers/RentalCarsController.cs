using GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models;
using GBC_TRAVEL_GROUP_88.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Controllers
{
    [Area("BookingManagement")]
    [Route("[area]/[controller]/[action]")]
    public class RentalCarsController : Controller
    {
        

        private readonly AppDbContext _db;
        private readonly ILogger<RentalCarsController> _logger;

        public RentalCarsController(AppDbContext db, ILogger<RentalCarsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var rentalCars = await _db.RentalCars.ToListAsync();
            return View(rentalCars);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var rentalCars = await _db.RentalCars.FirstOrDefaultAsync(h => h.RentalCarId == id);
            if (rentalCars == null)
            {
                return NotFound();
            }
            return View(rentalCars);
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
        public async Task<IActionResult> Create(RentalCar rentalCar)
        {
            if (ModelState.IsValid)
            {
                await _db.RentalCars.AddAsync(rentalCar);
                await _db.SaveChangesAsync();
                _logger.LogInformation("RentalCar created successfully: {RentalCarID}, {ModelName}", rentalCar.RentalCarId, rentalCar.ModelName);
                return RedirectToAction("Index");
            }

            return View(rentalCar);
        }

        [HttpGet("Edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var rentalCar = await _db.RentalCars.FindAsync(id);
            if (rentalCar == null)
            {
                return NotFound();
            }
            return View(rentalCar);
        }


        [HttpPost("Edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentalCarId, PlateNumber, RentalProvider, ModelName, ModelDescription, ModelType, Price, IsAvaiable")] RentalCar rentalCar)
        {
            _logger.LogInformation($"Received ID: {id}, RentalCarId: {rentalCar.RentalCarId}");
            if (id != rentalCar.RentalCarId)
            {
                //_logger.LogWarning("ID does not match RentalCarId");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(rentalCar); // no await is required here
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RentalCarExists(rentalCar.RentalCarId))
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
            return View(rentalCar);
        }

        private async Task<bool> RentalCarExists(int id)
        {
            return await _db.RentalCars.AnyAsync(e => e.RentalCarId == id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rentalCar = await _db.RentalCars.FirstOrDefaultAsync(r => r.RentalCarId == id);
            if (rentalCar == null)
            {
                return NotFound();
            }
            return View(rentalCar);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirm")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int RentalCarId)
        {
            var rentalCar = await _db.RentalCars.FindAsync(RentalCarId);
            if (rentalCar != null)
            {
                _db.RentalCars.Remove(rentalCar);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();


        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var rentalCarsQuery = from r in _db.RentalCars
                              select r;
            bool searchPerformed = !string.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                rentalCarsQuery = rentalCarsQuery.Where(r => r.PlateNumber.Contains(searchString)
                                                             || r.RentalProvider.Contains(searchString)
                                                             || r.ModelName.Contains(searchString)
                                                             || r.ModelType.Contains(searchString)
                                                             );
            }

            var rentalCars = await rentalCarsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", rentalCars);

        }

    }
}
