using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YesNoPuzzle.Data;
using YesNoPuzzle.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using YesNoPuzzle.Models;
using Microsoft.EntityFrameworkCore;

namespace YesNoPuzzle.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;

        public HomeController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel()
            {
                Games = _db.Games.ToList(),
            };
            return View(model);
        }

        public async Task<IActionResult> AddNewGame(GameViewModel model)
        {
            _db.Games.Add(new Game()
            {
                GameName = model.GameName,
                GameCondition = model.GameCondition,
                GameState = true,
                User = await _userManager.FindByNameAsync(User.Identity.Name),
            });
            await _db.SaveChangesAsync();
            return Content("OK");
        }

        public IActionResult Game(int id)
        {
            var model = _db.Games.Include(g=>g.Questions).FirstOrDefault(g => g.Id == id);
            return View(model);
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

        //*****************************************


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
