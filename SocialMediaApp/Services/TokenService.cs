using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.Entities;
using SocialMediaApp.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMediaApp.Services
{
	public class TokenService : ITokenService
	{
		// the Key
		private readonly SymmetricSecurityKey _Key;
        public TokenService(IConfiguration config)
        {
			_Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
		{
			// Create Claims
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.NameId , user.UserName)
			};
			// create the Signuture => key,Algorithm
			var creds = new SigningCredentials(_Key,SecurityAlgorithms.HmacSha512Signature);

			// Declares a variable named tokenDescriptor of type SecurityTokenDescriptor.
			// This object holds the configuration settings for creating a JWT.
			//يعمل هذا المتغير بمثابة صندوق يحفظ إعدادات تكوين عملية إنشاء رمز مميز JWT.

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = creds

			};
			//it's likely used for generating and handling JWTs.
			var tokenHandler = new JwtSecurityTokenHandler(); 

			//This method uses the information in the tokenDescriptor (claims, expiration, signing credentials)
			//to generate a JWT in its internal representation (likely a security token object).
			var token = tokenHandler.CreateToken(tokenDescriptor);

			// Serialize the token to json
			return tokenHandler.WriteToken(token);

		}
	}
}
