using GamblingSite.Infrastructure.Models.Roulette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Core.Interfaces
{
    public interface IRouletteService
    {
        Task<Roulette> Spin(decimal betAmount);
    }
}
