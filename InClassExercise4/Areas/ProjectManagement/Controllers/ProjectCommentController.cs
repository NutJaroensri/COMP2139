using WebApplication2.Areas.ProjectManagement.Models;
using WebApplication2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication2.Areas.ProjectManagement.Controllers
{
	[Area("ProjectManagement")]
	[Route("[area]/[controller]/[action]")]
	public class ProjectCommentController : Controller
	{
		private readonly AppDbContext _context;
		public ProjectCommentController(AppDbContext context)
		{
			_context = context;
		}

		// Get
		[HttpGet]
		public async Task<IActionResult> GetComments(int projectId) { 
			var comments = await _context.ProjectComments.Where(c => c.ProjectId == projectId)
											.OrderByDescending(c => c.CreatedDate).ToListAsync();
			return Json(comments);
		}
		// Post
		[HttpPost]
		public async Task<IActionResult> AddComment([FromBody] ProjectComment comment) { 
			if(ModelState.IsValid)
			{
				comment.CreatedDate = DateTime.Now; // Set current time
				_context.ProjectComments.Add(comment);
				await _context.SaveChangesAsync();
				return Json(new { success = true, message = "Comment added successfully."});
			}

			var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
			return Json(new { success = false, message = "Invalid comment data.", errors = errors });
		}
	}
}
