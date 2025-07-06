using GamblingSite.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Core.Interfaces
{
    public interface ISlotMachineService
    {
        SlotMachineResult Spin(decimal betAmount);
    }
}
