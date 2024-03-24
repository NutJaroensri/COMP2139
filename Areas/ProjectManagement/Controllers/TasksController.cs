using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Areas.ProjectManagement.Models;

namespace WebApplication2.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<TasksController> _logger;

        public TasksController(AppDbContext db, ILogger<TasksController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Get: Tasks
        [HttpGet("Index/{projectId:int}")]
        public async Task<IActionResult> Index(int? projectId)
        {
            /*var tasks = _db.ProjectTasks.Where(t => t.ProjectId == projectId).ToList();
            ViewBag.ProjectId = projectId;
            return View(tasks);*/
            var tasksQuery = _db.ProjectTasks.AsQueryable();

            if (projectId.HasValue) { 
                tasksQuery = tasksQuery.Where(t=> t.ProjectId == projectId.Value);
            }

            var tasks = await tasksQuery.ToListAsync();
            ViewBag.ProjectId = projectId;
            return View(tasks);
        }

        // Get: Tasks/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var taskQuery = _db.ProjectTasks.AsQueryable();
            var task = await _db.ProjectTasks
                            .Include(t => t.Project) // include related project data
                            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }


        [HttpGet("Create/{projectId:int}")]
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var task = new ProjectTask
            {
                ProjectId = projectId,
            };
            return View(task);


        }

        [HttpPost("Create/{projectId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                await _db.ProjectTasks.AddAsync(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        /*public IActionResult Edit(int id) {
            _logger.LogInformation("Attempting to edit task with ID: {Id}", id);
            var task = _db.ProjectTasks.Include(_t => _t.Project).FirstOrDefault(t => t.ProjectTaskId == id);

            if(task == null) {
                _logger.LogWarning("Task not found with ID: {Id}", id);
                return NotFound();
            }
            var projectName = task.Project?.Name;
            Console.WriteLine($"Project Name: {projectName}");

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        
        }*/


        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Attempting to edit task with ID: {Id}", id);
            var task = await _db.ProjectTasks.Include(_t => _t.Project).FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                _logger.LogWarning("Task not found with ID: {Id}", id);
                return NotFound();
            }

            // Populate ViewBag.Projects with a SelectList of projects
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);

            // Log project name for debugging
            if (task.Project != null)
            {
                _logger.LogInformation("Project name associated with task: {ProjectName}", task.Project.Name);
            }
            else
            {
                _logger.LogWarning("Task with ID {Id} does not have an associated project.", id);
            }

            return View(task);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound(id);
            }

            if (ModelState.IsValid)
            {
                _db.Update(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);


        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _db.ProjectTasks.
                        Include(t => t.Project)
                        .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);


        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ProjectTaskId)
        {
            var task = await _db.ProjectTasks.FindAsync(ProjectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            return NotFound();

        }

        // Tasks/Search?projectId=1&searchString=Task1
        [HttpGet("Search/{projectId:int}/{searchString?}")]
        public async Task<IActionResult> Search(int projectId, string searchString)
        {
            var tasksQuery = _db.ProjectTasks.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                tasksQuery = tasksQuery.Where(t => t.Title.Contains(searchString)
                                                        || t.Description.Contains(searchString));

            }

            var tasks = await tasksQuery.ToListAsync();
            ViewBag.ProjectId = projectId; // To keep track of the current project
            return View("Index", tasks);



        }


    }
}
