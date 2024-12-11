using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Dbcontext;
using MovieApi.DTO;
using MovieApi.Models;
using System.Globalization;

namespace MovieApi.Services
{
    public class UserServices : IuserServices
    {
        private readonly ApplicationDbcontext _context;

        public UserServices(ApplicationDbcontext context)
        {
            _context = context;
        }

   

        public async Task<string> RemoveUser(string name)
        {
            var user =  await _context.Users.SingleOrDefaultAsync(n=>n.UserName == name);
            if (user != null)
            {
               _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return "User Removed";

            }
            else
            {
                return "Not Found";
            }
        }


        public async Task<Appuser> upadteDetails(DtoNewuser dtoNewuser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == dtoNewuser.Id);
            if (user == null) return null;
            user.pass = dtoNewuser.Password;
            user.Email = dtoNewuser.Email;
            user.UserName= dtoNewuser.UserName;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public async  Task<List<DtoNewuser>> GetAllUsers()
        {
            
            var users=await _context.Users.ToListAsync();

            if (users.Count != 0)
            {
                var userspecificinfo = users.Select(x => new DtoNewuser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Id =x.Id,
                   // Role = x.role
                }).ToList();
                return userspecificinfo;
            }
            return null;
           
        }
        


    }
}
