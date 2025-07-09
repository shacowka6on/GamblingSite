using GamblingSite.Infrastructure.Models.SlotMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Core.Interfaces
{
    public interface ISlotMachineService
    {
        Task<SlotMachine> Spin(decimal betAmount, int id);
    }
}
