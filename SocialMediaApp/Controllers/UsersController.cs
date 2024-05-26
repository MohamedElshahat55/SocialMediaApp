using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.DTOs;
using SocialMediaApp.Entities;
using SocialMediaApp.Interfaces;

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

		[AllowAnonymous]
		[HttpGet("GetAllUsers")]
		public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllUsers()
		{
			var users = await _userRepository.GetAllUsersAsync();
			var userReturnded = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(userReturnded);
		}

	
		[HttpGet("GetUserById/{id}")]
		public async Task<ActionResult<AppUser>> GetUserById(int id)
		{
			return Ok(await _userRepository.GetByIdAsync(id));
		}

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByName(string username)
        {
			var user = await _userRepository.GetUserByNameAsync(username);
            var userReturnded = _mapper.Map<MemberDto>(user);
            return Ok(userReturnded);
        }
    }
}
