using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Bson;
using RescueRide.Application.Services;
using RescueRide.Core.Models;
using RescueRide.Infrastructure.Data;

namespace RescueRide.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(AppDbContext context, MongoDbService mongoDbService)
        {
            _context = context;
            _usersCollection = mongoDbService.GetCollection<User>("Users");
        }

        public async Task AddUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Id) || !ObjectId.TryParse(user.Id, out _))
            {
                user.Id = ObjectId.GenerateNewId().ToString();
            }
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _usersCollection.Find(u => u.Name == username).FirstOrDefaultAsync();
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await GetUserByEmailAsync(username);
            return user != null && user.PasswordHash == password;
        }
    }

}
