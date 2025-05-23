﻿using EFCoreWebApi.Data;
using EFCoreWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreWebApi.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User?> SearchSingle(string userName)
        {
            return await _context.Users
                                 .Where(u => u.UserName == userName || u.Email == userName)
                                 .SingleOrDefaultAsync();
        }

        public async Task AddAsync(User user)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false; // Product not found
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> Search(string propertyName, string searchText)
        {
            IQueryable<User> query = _context.Users;

            switch (propertyName)
            {
                case nameof(User.Name):
                    query = query.Where(x => x.Name == searchText);
                    break;
                case nameof(User.Email):
                    query = query.Where(x => x.Email == searchText);
                    break;
                case nameof(User.UserName):
                    query = query.Where(x => x.UserName == searchText);
                    break;
                default:
                    throw new ArgumentException($"Property '{propertyName}' is not supported for searching.");
            }
            var result = await query.ToListAsync();
            return result;
        }

    }
}