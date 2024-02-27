using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("Projects")]
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ProjectsController> _logger;
        public ProjectsController(AppDbContext db, ILogger<ProjectsController> logger) {
            _db = db;
            _logger = logger;
        }
        /*public ProjectsController(AppDbContext db)
        {
            _db = db;
        }*/

        [HttpGet("")]
        public IActionResult Index()
        {
            /*var projects = new List<Project>() {
                new Project { Projectid = 1, Name = "Project 1", Description = "First Project" },
                new Project { Projectid = 2, Name = "Project 2", Description = "Second Project" }

            };

            return View(projects);*/
            return View(_db.Projects.ToList());
        }

        [HttpGet("Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
            if(project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project) {
            if (ModelState.IsValid) { 
                _db.Projects.Add(project);
                _db.SaveChanges();
                //_logger.LogInformation("Project created successfully: {ProjectId}, {Name}", project.Projectid, project.Name);
                return RedirectToAction("Index");
            }
            /*else
            {
                // Log model state errors
                _logger.LogWarning("Project creation failed due to model state errors:");
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning(modelError.ErrorMessage);
                }
            }*/
            return View(project);
        }

        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var project = _db.Projects.Find(id);
            if(project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectId, Name, Description, StartDate, EndDate, Status")] Project project)
        {

            //_logger.LogInformation($"Received ID: {id}, ProjectId: {project.Projectid}");
            if (id != project.ProjectId)
            {

                //_logger.LogWarning("ID does not match ProjectId");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(project);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            return View(project);
        }

        private bool ProjectExists(int id) { 
            return _db.Projects.Any(e => e.ProjectId == id);
        }

        [HttpGet("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
            if(project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirm")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int ProjectId) {
            var project = _db.Projects.Find(ProjectId);
            if(project != null)
            {
                _db.Projects.Remove(project);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        
        
        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString) {
            var projectsQuery = from p in _db.Projects
                               select p;
            bool searchPerformed = !String.IsNullOrEmpty(searchString);
            if (searchPerformed) { 
                projectsQuery = projectsQuery.Where(p => p.Name.Contains(searchString) 
                                                             || p.Description.Contains(searchString));
            }

            var projects = await projectsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", projects);
        
        }
    }
}
