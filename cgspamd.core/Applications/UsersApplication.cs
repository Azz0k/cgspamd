using cgspamd.core.Contexts;
using cgspamd.core.Models;
using cgspamd.core.Models.APIModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static cgspamd.core.Utils.Utils;

namespace cgspamd.core.Applications
{
    public class UsersApplication
    {
        private StoreDbContext db;
        private string deletedUserPrefix = "_deleted_";
        public UsersApplication(StoreDbContext storeDbContext) 
        {
            db = storeDbContext;
        }
        public async Task<List<UserDTO>> GetAllRecordsAsync()
        {
            return await db.Users
                .Where(x => !x.Deleted)
                .Select(x => new UserDTO()
            {
                Id = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                Enabled = x.Enabled,
                IsAdmin = x.IsAdmin,
            }).ToListAsync();
        }
        public async Task<int> AddAsync(AddUserRequest request)
        {

            if (!isAddUserRequestValid(request))
            {
                return -1;
            }
            User? existingUser = await db.Users.FirstOrDefaultAsync(user => user.UserName == request.UserName);
            if (existingUser != null)
            {
                return existingUser.Id;
            }
            string hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User newUser = new() { UserName = request.UserName, Hash = hash, FullName = request.FullName, Enabled = request.Enabled, IsAdmin = request.IsAdmin };
            await db.Users.AddAsync(newUser);
            await db.SaveChangesAsync();
            return newUser.Id;
        }
        public async Task<int> UpdateAsync(UpdateUserRequest request)
        {
            if (!isUpdateUserRequestValid(request)) return 400;
            try
            {
                User? existingUser = await db.Users.FindAsync(request.Id);
                if (existingUser == null || existingUser.Deleted)
                {
                    return 404;
                }
                existingUser.UserName = request.UserName;
                existingUser.FullName = request.FullName;
                existingUser.Enabled = request.Enabled;
                existingUser.IsAdmin = request.IsAdmin;
                if (request.Password != null)
                {
                    string hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    existingUser.Hash = hash;
                }
                await db.SaveChangesAsync();
            }
            catch
            {
                return 400;
            }
            return 200;
        }
        public async Task<int> DeleteAsync(int id)
        {
            if (id <= 0) return 400;
            var existingUser = await db.Users.FindAsync(id);
            if (existingUser == null || existingUser.Deleted) return 404;
            existingUser.UserName = deletedUserPrefix+existingUser.UserName;
            existingUser.Deleted = true;
            await db.SaveChangesAsync();
            return 204;
        }
    }
}
