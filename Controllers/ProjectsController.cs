using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProjectsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var projects = new List<Project>() { 
                new Project { Project_id = 1, Name = "Project 1", Description = "First Project" },
                new Project { Project_id = 2, Name = "Project 2", Description = "Second Project"}
            };
            return View(projects);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = new Project { Project_id = id, Name = "Project " + id, Description = "Details of project " + id };
            return View(project);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            return RedirectToAction("Index");
        }
    }
}
