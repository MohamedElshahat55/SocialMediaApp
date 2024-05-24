using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Data;
using SocialMediaApp.Entities;

namespace SocialMediaApp.Controllers
{
    public class BuggyController : ApiBaseController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize] // => indicating it requires authorization.
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret Text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.AppUsers.Find(-1);
            if(thing == null) return NotFound();
            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            var thing = _context.AppUsers.Find(-1);
            var thingToReturn = thing.ToString(); //error
            return Ok(thingToReturn);
        }


        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            
            return BadRequest("this was not a good request");
        }

        
    }
}
