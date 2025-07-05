using GamblingSite.Core.DTOs;
using GamblingSite.Infrastructure.Data;
using GamblingSite.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt;
using GamblingSite.Core.Interfaces;

namespace GamblingSite.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly GamblingSiteDbContext _context;
        private readonly JwtTokenService _tokenService;
        public AuthService(GamblingSiteDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto> Register(RegisterUserDto dto)
        {
            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email);
            
            if (emailExists)
            {
                throw new ArgumentException("Email already registered");
            }

            var user = new User()
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Balance = 100,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Balance = user.Balance,
            };
        }
        public async Task<string> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) 
            {
                throw new ArgumentException("Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new ArgumentException("Invalid email or password");
            }
            var token = _tokenService.GenerateToken(user);
            return token;
        }
    }
}
