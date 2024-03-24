using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Areas.ProjectManagement.Models;

namespace WebApplication2.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ProjectsController> _logger;
        public ProjectsController(AppDbContext db, ILogger<ProjectsController> logger)
        {
            _db = db;
            _logger = logger;
        }
        /*public ProjectsController(AppDbContext db)
        {
            _db = db;
        }*/

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var projects = await _db.Projects.ToListAsync();
            return View(projects);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
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
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                await _db.Projects.AddAsync(project);
                await _db.SaveChangesAsync();
                //_logger.LogInformation("Project created successfully: {ProjectId}, {Name}", project.Projectid, project.Name);
                return RedirectToAction("Index");
            }
            
            return View(project);
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId, Name, Description, StartDate, EndDate, Status")] Project project)
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
                    _db.Update(project); // no await is required here
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProjectExists(project.ProjectId))
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

        private async Task<bool> ProjectExists(int id)
        {
            return await _db.Projects.AnyAsync(e => e.ProjectId == id);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ProjectId)
        {
            var project = await _db.Projects.FindAsync(ProjectId);
            if (project != null)
            {
                _db.Projects.Remove(project);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();


        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var projectsQuery = from p in _db.Projects
                                select p;
            bool searchPerformed = !string.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
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
