using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YesNoPuzzle.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YesNoPuzzle.Models;
using YesNoPuzzle.Models.GameViewModels;
using System;


namespace YesNoPuzzle.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        public PlayerController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null)
            {
               List<Game> games = await _db.Games.ToListAsync();   
                            
                games.Sort(delegate (Game g1, Game g2)
                { return g1.GameName.CompareTo(g2.GameName); });

                return View(games);
            }
            return RedirectToAction("Index", "Admin");
        }

        public IActionResult CreateNewGame()
        {
            // если пользователь незарегистрированный
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null)
                return View();
            else
                return RedirectToAction("CreateNewGame", "Admin");
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
            var game = _db.Games.SingleOrDefault(g => g.Id == gameId);

            var user = _db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);

            if (game == null)
            {
                return NotFound();
            }

            if (text != null)
                _db.Questions.Add(new Question
                {
                    Text = text,
                    State = 0,
                    Game = game,
                    QuestionDate = DateTime.Now,
                    UserName = (user == null) ? "Anonimus" : user.Email
                });
            
            await _db.SaveChangesAsync();
           
            return RedirectToAction("Game", new { id = gameId });

        }

        public async Task<IActionResult> Rating()
        {
            List<ApplicationUser> users = await _db.Users.ToListAsync();

            users.Sort(delegate (ApplicationUser u1, ApplicationUser u2)
            { return u2.SolvedGamesCount.CompareTo(u1.SolvedGamesCount); });

            return View(users);
        }

        public IActionResult Question()
        {
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                return RedirectToAction("Question", "Admin");
            }

            return View();
        }


        public IActionResult Rules()
        {
            ViewData["Message"] = "YesNo Puzzle.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Our contact page.";

            return View();
        }      
    }
}
