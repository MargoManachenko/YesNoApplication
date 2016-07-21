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

        public async Task<IActionResult> CreateNewGame()
        {
            return View();
        }

        public async Task<IActionResult> AddNewGame(GameViewModel model)
        {
            _db.Games.Add(new Game()
            {
                GameName = model.GameName,
                GameCondition = model.GameCondition,
                GameState = true,
                User = await _userManager.GetUserAsync(HttpContext.User)
            });
            await _db.SaveChangesAsync();
            //Content("OK");
            //ViewBag.Msg = "ok";
            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> DeleteGame(int? id)
        {
            if (id == null)
                return NotFound();

            var game = _db.Games.First(g => g.Id == id);

            var questions = _db.Questions.Where(q => q.Game == game).ToList();

            foreach (var q in questions)
            {
                _db.Questions.Remove(q);
            }

            _db.Games.Remove(game);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> Game(int? id)
        {
            if (id == null)
                return NotFound();

            var game = await _db.Games.Include(g => g.Questions).SingleOrDefaultAsync(g => g.Id == id);

            var gameViewModel = new GameViewModel
            {
                GameId = game.Id,
                Game = game,
                Questions = game.Questions
            };

            return View(gameViewModel);
        }

        public async Task<IActionResult> AddNewQuestion(string text,int gameId)
        {
            var game = _db.Games.FirstOrDefault(g => g.Id == gameId);

            if (game == null)
            {
                return NotFound();
            }
            _db.Questions.Add(new Question
            {
                Text = text,
                State = 0,
                Game = game
            });
            await _db.SaveChangesAsync();
            //return Content("OK");
            return RedirectToAction(nameof(Index), "Home");

        }

        public async Task<IActionResult> Question()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var games = await _db.Games
                .Include(g => g.Questions)
                .Where(g => g.User.Id == userId && g.GameState)
                .ToListAsync();


            foreach (var t in games)
                t.Questions = t.Questions.Where(q => q.State == 0).ToList();

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
