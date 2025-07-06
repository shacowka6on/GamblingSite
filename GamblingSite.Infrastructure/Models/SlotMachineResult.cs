using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Infrastructure.Models
{
    public class SlotMachineResult
    {
        public string[] Symbols { get; set; }  
        public decimal WinAmount { get; set; }
        public bool isJackpot { get; set; } 
    }
}
