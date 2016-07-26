using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YesNoPuzzle.Data;
using YesNoPuzzle.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Core.v3;
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
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null)
            {
                var games = await _db.Games.ToListAsync();
                return View(games);
            }

            return RedirectToAction("Index", "Game");

        }

        public async Task<IActionResult> CreateNewGame()
        {
            return View();
        }

        public async Task<IActionResult> AddNewGame(Models.GameViewModels.GameViewModel model)
        {
            if (model == null) return NotFound();

            _db.Games.Add(new Game()
            {
                GameName = model.GameName,
                GameCondition = model.GameCondition,
                GameState = true,
                User = await _userManager.GetUserAsync(HttpContext.User)
            });
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Game");
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

            var gameViewModel = new Models.GameViewModels.GameViewModel
            {
                GameId = game.Id,
                Game = game,
                Questions = game.Questions
            };

            return View(gameViewModel);
        }

        public async Task<IActionResult> AddNewQuestion(string text, int gameId)
        {
            var game = _db.Games.FirstOrDefault(g => g.Id == gameId);

            if (game == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(text))
            {
                 _db.Questions.Add(new Question
            {
                Text = text,
                State = 0,
                Game = game
            });
            await _db.SaveChangesAsync();
            } 

            var adress = "Game/" + gameId;

            return RedirectToAction(adress, "Home");

        }

        public async Task<IActionResult> Question(int? id)
        {
            var games = await _db.Games
                .Include(g => g.Questions)
                .Where(g => g.Id == id && g.GameState)
                .ToListAsync();

            return View(games);
        }

        public async Task<IActionResult> AnswerYes(int? id)
        {
            if (id == null)
                return NotFound();

            var question = _db.Questions.First(q => q.Id == id);

            question.State = 1;

            await _db.SaveChangesAsync();

            return RedirectToAction("Question", "Home");
        }

        public async Task<IActionResult> AnswerNo(int? id)
        {
            if (id == null)
                return NotFound();

            var question = _db.Questions.First(q => q.Id == id);

            question.State = 2;

            await _db.SaveChangesAsync();

            return RedirectToAction("Question", "Home");
        }


        public async Task<IActionResult> AnswerNoMatter(int? id)
        {
            if (id == null)
                return NotFound();

            var question = _db.Questions.First(q => q.Id == id);

            question.State = 3;

            await _db.SaveChangesAsync();

            return RedirectToAction("Question", "Home");
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
