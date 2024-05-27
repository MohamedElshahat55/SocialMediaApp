using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.DTOs;
using SocialMediaApp.Entities;
using SocialMediaApp.Interfaces;
using System.Runtime.ExceptionServices;
using System.Security.Claims;

namespace SocialMediaApp.Controllers
{
	[Authorize]
	public class UsersController : ApiBaseController
	{
        private readonly IRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IRepository userRepository , IMapper mapper)
        {
           _userRepository = userRepository;
            _mapper = mapper;
        }

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllUsers()
		{
			var users = await _userRepository.GetAllUsersAsync();
			var userReturnded = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(userReturnded);
		}

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByName(string username)
        {
			var user = await _userRepository.GetUserByNameAsync(username);
            var userReturnded = _mapper.Map<MemberDto>(user);
            return Ok(userReturnded);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatedMember(MemberUpdatedDTO memberUpdatedDTO)
        {
            // 1) we want to find the user name of this user?
            // how can i find it?! => get it from claims
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // 2) retrive the user from database basedon this username
            var user = await _userRepository.GetUserByNameAsync(username);
            if(user == null) return NotFound();
            // 3) use Auto Mapper
            _mapper.Map(memberUpdatedDTO, user);
            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed To Update user");
        }
    }
}
