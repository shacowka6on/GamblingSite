using GamblingSite.Infrastructure.Data;
using GamblingSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamblingSite.Controllers
{
    public class UserController : Controller
    {
        private readonly GamblingSiteDbContext _context;
        public UserController(GamblingSiteDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateUser), new { id = user.Id}, user);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> CreateBet(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            
            _context.Entry(user).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
