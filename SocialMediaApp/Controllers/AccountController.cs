using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.DTOs;
using SocialMediaApp.Entities;
using SocialMediaApp.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SocialMediaApp.Controllers
{
	public class AccountController : ApiBaseController
	{
		private readonly DataContext _dbcontext;
		private readonly ITokenService _tokenService;

		public AccountController(DataContext dbcontext , ITokenService tokenService)
        {
			_dbcontext = dbcontext;
			_tokenService = tokenService;
		}
		[HttpPost("register")] // /api/Account/register
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			// Check the user is exist in database
			if (await CheckUserIsExist(registerDto.UserName)) return BadRequest("UserName Is Token");

			using var hmac = new HMACSHA512();
			var User = new AppUser()
			{
				UserName = registerDto.UserName.ToLower(),
				PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
				PasswordSalt = hmac.Key
			};
			
			_dbcontext.AppUsers.Add(User);
			await _dbcontext.SaveChangesAsync();

			return new UserDto
			{
				Username = User.UserName,
				Token = _tokenService.CreateToken(User)
			};
		}

		private async Task<bool> CheckUserIsExist(string userName)
		{
			return await _dbcontext.AppUsers.AnyAsync(u=>u.UserName == userName.ToLower());
		}

		[HttpPost("login")] // /api/Account/login

		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var User = await _dbcontext.AppUsers.SingleOrDefaultAsync(u=>u.UserName ==  loginDto.UserName);
			if(User == null) return Unauthorized();
			// You have to Check Password
			// Pass PasswordSalt tot this fn
			using var hmac = new HMACSHA512(User.PasswordSalt);
			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); //retun array of bytes

			for(int i = 0; i<computedHash.Length; i++)
			{
				if (computedHash[i] != User.PasswordHash[i]) return Unauthorized("Invalid password");
			}
			return new UserDto
			{
				Username = User.UserName,
				Token = _tokenService.CreateToken(User)
			};

		}
	}
}
