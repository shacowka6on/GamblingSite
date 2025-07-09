using GamblingSite.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GamblingSite.Controllers
{
    public class SlotMachineController : Controller
    {
        private readonly ISlotMachineService _slotMachineService;
        public SlotMachineController(ISlotMachineService slotService)
        {
            _slotMachineService = slotService;
        }
        [HttpPost("spin")]
        public IActionResult Spin([FromBody] decimal betAmount, int id)
        {
            try
            {
                var result = _slotMachineService.Spin(betAmount, id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
