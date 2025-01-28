using RescueRide.Data;
using RescueRide.Models;
using Microsoft.EntityFrameworkCore;
using System;
using MongoDB.Driver;
using RescueRide.Services;
using MongoDB.Bson;
using RescueRide.Controllers;
using System.Reflection.Metadata;

namespace RescueRide.Repositories
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
                // Generate a new ObjectId if the Id is null, empty, or invalid
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }

}
