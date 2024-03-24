using Microsoft.AspNetCore.Mvc;
using WebApplication2.Areas.ProjectManagement.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;

namespace WebApplication2.Areas.ProjectManagement.Component.ProjectSummary
{
    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public ProjectSummaryViewComponent(AppDbContext context) { 
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId) { 
            var project = await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.ProjectId == projectId);
            if(project == null)
            {
                return Content("Project not found/");
            }
            return View(project);
        }
    }
}
