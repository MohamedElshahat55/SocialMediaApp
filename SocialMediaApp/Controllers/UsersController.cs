using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Entities;

namespace SocialMediaApp.Controllers
{
	[Authorize]
	public class UsersController : ApiBaseController
	{
		private readonly DataContext _dbcontext;

		public UsersController(DataContext dbcontext)
        {
			_dbcontext = dbcontext;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<AppUser>>> GetAllUsers()
		{
			var users = await _dbcontext.AppUsers.ToListAsync();
			return Ok(users);
		}

	
		[HttpGet("{id}")]
		public async Task<ActionResult<AppUser>> GetUserById(int id)
		{
			var user = await _dbcontext.AppUsers.FindAsync(id);
			return Ok(user);
		}
	}
}
