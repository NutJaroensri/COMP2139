using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FlightController : Controller
    {
            private readonly AppDbContext _context;

            public FlightController(AppDbContext context)
            {
                _context = context;
            }

            public IActionResult Index()
            {
                return View(_context.Flights.ToList());
            }

            [HttpGet]
            public IActionResult Details(int id)
            {
                var flight = _context.Flights.FirstOrDefault(p => p.Id == id);
                if (flight == null)
                {
                    return NotFound();
                }
                return View(flight);
            }

            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Flight flight)
            {
                if (ModelState.IsValid)
                {
                    _context.Flights.Add(flight);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(flight);
            }

            [HttpGet]
            public IActionResult Edit(int id)
            {
                var flight = _context.Flights.Find(id);
                if (flight == null)
                {
                    return NotFound();
                }
                return View(flight);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(int id, Flight flight)
            {
                if (id != flight.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Flights.Update(flight);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FlightExists(flight.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return View(flight);
            }

            [HttpGet]
            public IActionResult Delete(int id)
            {
                var flight = _context.Flights.FirstOrDefault(p => p.Id == id);
                if (flight == null)
                {
                    return NotFound();
                }
                return View(flight);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int id)
            {
                var flight = _context.Flights.Find(id);
                if (flight != null)
                {
                    _context.Flights.Remove(flight);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }

            [HttpGet]
            public async Task<IActionResult> Search(string origin, string destination, TimeOnly? departureTime, decimal? price)
            {
                var flightQuery = from p in _context.Flights
                                  select p;

                bool searchPerformed = !String.IsNullOrEmpty(origin)
                                    || !String.IsNullOrEmpty(destination)
                                    || departureTime != null
                                    || price != null;

                if (searchPerformed)
                {
                    if (!String.IsNullOrEmpty(origin))
                    {
                        flightQuery = flightQuery.Where(p => p.Origin.Contains(origin));
                    }

                    if (!String.IsNullOrEmpty(destination))
                    {
                        flightQuery = flightQuery.Where(p => p.Destination.Contains(destination));
                    }


                }

                var flights = await flightQuery.ToListAsync();
                ViewData["SearchedPerformed"] = searchPerformed;
                return View("Index", flights);
            }

            public IActionResult Book(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var flight = _context.Flights.FirstOrDefault(m => m.Id == id);
                if (flight == null)
                {
                    return NotFound();
                }

                return View(flight);
            }


            [HttpPost]
            public IActionResult Book(int id, [Bind("Id,Name")] FlightBooking flightBooking)
            {
                if (ModelState.IsValid)
                {
                    _context.FlightBookings.Add(flightBooking);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

                return View(flightBooking);
            }



            private bool FlightExists(int id)
            {
                return _context.Flights.Any(p => p.Id == id);
            }
    }
}
