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
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        public GameController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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

        public async Task<IActionResult> DeleteQuestion(int? id)
        {
            if (id == null)
                return NotFound();

            var question = _db.Questions.First(q => q.Id == id);
            
            _db.Questions.Remove(question);

            await _db.SaveChangesAsync();

            return RedirectToAction("Question", "Game");
        } 

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var games = await _db.Games.Where(u => u.User.Id == userId).ToListAsync();

            return View(games);
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

        public async Task<IActionResult> GameOver(int? id)
        {
            if (id == null)
                return NotFound();

            var game = _db.Games.First(g => g.Id == id);

            game.GameState = false;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home");
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
