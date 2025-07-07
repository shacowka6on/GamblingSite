using GamblingSite.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
