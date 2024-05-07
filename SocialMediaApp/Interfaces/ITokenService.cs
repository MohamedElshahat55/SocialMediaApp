using SocialMediaApp.Entities;

namespace SocialMediaApp.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(AppUser user);
	}
}
