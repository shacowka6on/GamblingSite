using GamblingSite.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Core.Interfaces
{
    public interface IAuthService
    {
        Task<UserProfileDto> Register(RegisterUserDto dto);
        Task<string> Login(string email, string password);
    }
    
}
