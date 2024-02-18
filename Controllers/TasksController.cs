using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;
        public TasksController(AppDbContext db) {
            _db = db;
        }
        [HttpGet]
        public IActionResult Index(int projectId)
        {
            var tasks = _db.ProjectTasks.Where(t => t.ProjectId == projectId).ToList();
            ViewBag.ProjectId = projectId;
            return View(tasks);
        }
        [HttpGet]
        public IActionResult Details(int id) {
            var task = _db.ProjectTasks.Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);
            if (task == null) {
                return NotFound();
            }
            return View(task);
        
        }

        public IActionResult Create(int id) {
            var project = _db.Projects.Find(id);
            if (project == null) {
                return NotFound();
            }

            var task = new ProjectTask
            {
                ProjectId = id
            };

            return View(task);
       
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Descrition", "ProjectId")] ProjectTask task) {
            if (ModelState.IsValid) {
                _db.ProjectTasks.Add(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        public IActionResult Edit(int id) {
            var task = _db.ProjectTasks.Include(t => t.Project).FirstOrDefault(t => t.ProjectTaskId == id);
            if(task == null)
            {
                return NotFound();
            }

            ViewBag.Project = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title","Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId) {
                return NotFound();
            }
            if(ModelState.IsValid){
                _db.Update(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }
        public IActionResult Delete(int id)
        {
            var task = _db.ProjectTasks.Include(t => t.Project).FirstOrDefault(t => t.ProjectTaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);

        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int ProjectTaskId)
        {
            var task = _db.ProjectTasks.Find(ProjectTaskId);
            if (task != null) {
                _db.ProjectTasks.Remove(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            return NotFound();
        }
    }
}
