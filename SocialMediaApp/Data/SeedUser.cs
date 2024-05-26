using SocialMediaApp.Data;
using SocialMediaApp.Entities;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.DataSeeding
{
    public static class SeedUser
    {
        public static async Task SeedAsync(DataContext _dbcontext)
        {
            if (await _dbcontext.AppUsers.AnyAsync()) return;

            var usersData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(usersData);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$0rd"));
                user.PasswordSalt = hmac.Key;

                _dbcontext.AppUsers.Add(user);
            }

            await _dbcontext.SaveChangesAsync();
        }
    }
}
