using GamblingSite.Core.Interfaces;
using GamblingSite.Infrastructure.Data;
using GamblingSite.Infrastructure.Models.Blackjack;
using Microsoft.AspNetCore.Mvc;

namespace GamblingSite.Controllers
{
    public class BlackjackController : Controller
    {
        private readonly IBlackjackService _blackjackService;
        public BlackjackController(IBlackjackService blackjackService)
        {
            _blackjackService = blackjackService;
        }
        [HttpPost("start")]
        public IActionResult Start(int userId, decimal betAmount)
        {
            try
            {
                var game = _blackjackService.Start(userId, betAmount);
                return Ok(game);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
