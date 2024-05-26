using SocialMediaApp.Entities;

namespace SocialMediaApp.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();

        Task<AppUser> GetByIdAsync(int id);

        void Update(AppUser user);

        Task<bool> SaveAllAsync();

        Task<AppUser> GetUserByNameAsync(string username);
    }
}
