using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YesNoPuzzle.Data;
using YesNoPuzzle.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YesNoPuzzle.Models;
using YesNoPuzzle.Models.GameViewModels;

namespace YesNoPuzzle.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Game> games = await _db.Games.ToListAsync();
            return View(games);
        }

        public async Task<IActionResult> AddNewGame(IndexViewModel model)
        {
            _db.Games.Add(new Game()
            {
                GameName = model.GameName,
                GameCondition = model.GameCondition,
                GameState = true,
                User = await _userManager.GetUserAsync(HttpContext.User)
        });
            await _db.SaveChangesAsync();
            return Content("OK");
        }

        public async Task<IActionResult> Game(int? id)
        {
            if (id == null)
                return NotFound();

            var games = await _db.Games.Include(g => g.Questions).SingleOrDefaultAsync(g => g.Id == id);

            return View(games);
        }

        public async Task<IActionResult> AddNewQuestion(QuestionViewModel model)
        {
            var game = _db.Games.FirstOrDefault(g => g.Id == model.GameId);
            if(game == null)
            {
                return Json($"Game {model.GameId} not found");
            }
            _db.Questions.Add(new Question()
            {
                Text = model.Text,
                State = 0,
                Game = game
            });
            await _db.SaveChangesAsync();
            return Content("OK");
        }

        public async Task<IActionResult> Question()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var games = await _db.Games
                .Include(g => g.User)
                .Include(g => g.Questions)
                .Where(g => g.User.Id == userId)
                .ToListAsync();

            return View(games);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
