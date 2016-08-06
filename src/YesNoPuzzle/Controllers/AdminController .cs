using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YesNoPuzzle.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YesNoPuzzle.Models;
using YesNoPuzzle.Models.GameViewModels;
using Microsoft.AspNetCore.Routing;
using System;

namespace YesNoPuzzle.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> DeleteGame(int? id)
        {
            if (id == null)
                return NotFound();

            var game = _db.Games.SingleOrDefault(g => g.Id == id);

            var questions = _db.Questions.Where(q => q.Game == game).ToList();

            foreach (var q in questions)
            {
                _db.Questions.Remove(q);
            }

            _db.Games.Remove(game);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Admin");
        }

        public async Task<IActionResult> GameOver(int? id, string userName)
        {
            if (id == null || userName == null)
                throw new Exception("Dubasja");

            var game = _db.Games.SingleOrDefault(g => g.Id == id);

            var user = _db.Users.SingleOrDefault(u => u.UserName == userName);

            if(user != null)
            user.SolvedGamesCount++;

            game.GameState = false;

            await _db.SaveChangesAsync();

            return RedirectToAction("Question", "Admin");
        }
        public IActionResult CreateNewGame()
        {
            return View();
        }

        public async Task<IActionResult> EditGame(int? id)
        {
            if (id == null)
                return NotFound();

            var game = await _db.Games.Include(g => g.Questions).SingleOrDefaultAsync(g => g.Id == id);          

            return View(game);
        }

        public async Task<IActionResult> AddNewGame(GameViewModel model)
        {
            var user = _db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            if (model.GameCondition != null && model.GameName != null)
                _db.Games.Add(new Game()
                {
                    GameName = model.GameName,
                    GameCondition = model.GameCondition,
                    GameSolution = model.GameSolution, 
                    GameState = true,
                    User = await _userManager.GetUserAsync(HttpContext.User),
                    UserName = user.Email
                });
            
         
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Admin");
        }        
        public async Task<IActionResult> EditGameAction(int id, GameViewModel model)
        {            
            var game = _db.Games.SingleOrDefault(g => g.Id == id);
                      
            if (model.GameCondition != null && model.GameName != null)
            {
                game.GameName = model.GameName;
                game.GameCondition = model.GameCondition;
                game.GameSolution = model.GameSolution;
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Admin");
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;              
                
                var games = await _db.Games.Where(u => u.User.Id == userId).ToListAsync();

                games.Sort(delegate (Game g1, Game g2)
                { return g1.GameName.CompareTo(g2.GameName); });

                return View(games);
            }
            return View();    
        }

        public IActionResult ShowAllGames()
        {
            var games = _db.Games.ToList();

            games.Sort(delegate (Game g1, Game g2)
            { return g1.GameName.CompareTo(g2.GameName); });

            return View(games);
        }      

        public async Task<IActionResult> AnswerYes(int? id)
        {
            if (id == null)
                return NotFound();

            Question question = _db.Questions.SingleOrDefault(q => q.Id == id);

            question.State = 1;

            await _db.SaveChangesAsync();

            return RedirectToAction("Question", "Admin");
        }

        public async Task<IActionResult> AnswerNo(int? id)
        {
            if (id == null)
                return NotFound();

            Question question = _db.Questions.SingleOrDefault(q => q.Id == id);

            question.State = 2;

            await _db.SaveChangesAsync();

            return RedirectToAction("Question", "Admin");
        }

        public async Task<IActionResult> AnswerNoMatter(int? id)
        {
            if (id == null)
                return NotFound();

            Question question = _db.Questions.SingleOrDefault(q => q.Id == id);

            question.State = 3;

            await _db.SaveChangesAsync();

            return RedirectToAction("Question", "Admin");
        }

        public async Task<IActionResult> AnswerYesEdit(int? iD)
        {
            if (iD == null)
                return NotFound();

            Question question = _db.Questions.SingleOrDefault(q => q.Id == iD);            

            question.State = 1;

            await _db.SaveChangesAsync();

            return RedirectToAction("EditGame", "Admin", new { id = question.GameId });
        }

        public async Task<IActionResult> AnswerNoEdit(int? id)
        {
            if (id == null)
                return NotFound();

            Question question = _db.Questions.SingleOrDefault(q => q.Id == id);

            question.State = 2;

            await _db.SaveChangesAsync();

            return RedirectToAction("EditGame", "Admin", new { id = question.GameId });
        }

        public async Task<IActionResult> AnswerNoMatterEdit(int? id)
        {
            if (id == null)
                return NotFound();

            Question question = _db.Questions.SingleOrDefault(q => q.Id == id);

            question.State = 3;

            await _db.SaveChangesAsync();

            return RedirectToAction("EditGame", "Admin", new { id = question.GameId });
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

        public async Task<IActionResult> DeleteQuestion(int? id)
        {
            if (id == null)
                return NotFound();

            var question = _db.Questions.SingleOrDefault(q => q.Id == id);

            _db.Questions.Remove(question);

            await _db.SaveChangesAsync();

            return RedirectToAction("Question", "Admin");
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
