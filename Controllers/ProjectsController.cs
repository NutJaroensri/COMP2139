using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class ProjectsController : Controller
    {
        
        private readonly AppDbContext _db;
        public ProjectsController(AppDbContext db) {
            _db = db;
        }
        public IActionResult Index()
        {
            
            /*var projects = new List<Project>() { 
                new Project { Project_id = 123, Name = "weqw", Description = "First Project" },
                new Project { Project_id = 234, Name = "wefwef", Description = "Second Project"}
            };
            return View(projects);*/
            return View(_db.Projects.ToList());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = _db.Projects.FirstOrDefault(p => p.Project_id == id);
            if (project == null) {
                return NotFound();
            }
            return View(project);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid) {
                _db.Projects.Add(project);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public IActionResult Edit(int id)
        {
            var project = _db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Project_id, Name, Description, StartDate, EndDate, Status")] Project project)
        {
            if (id != project.Project_id) {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(project);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ProjectExists(project.Project_id))
                    {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                
                
                }
                return RedirectToAction(nameof(Index));
            
            }
            return View(project);
        }

        public bool ProjectExists(int id) {
            return _db.Projects.Any(e => e.Project_id == id);
        }

        public IActionResult Delete(int id)
        {
            var project = _db.Projects.FirstOrDefault(p => p.Project_id == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int projectId) {
            var project = _db.Projects.Find(projectId);
            if (project != null) { 
                _db.Projects.Remove(project);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
