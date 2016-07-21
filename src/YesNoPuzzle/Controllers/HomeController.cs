using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YesNoPuzzle.Data;

namespace YesNoPuzzle.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> AddNewQuestion(QuestionViewModel model)
        {
            var model = _db.Users.Where(u => u.GameState);
            return View(model);
        }

        public IActionResult Game(string id)
        {
            return Content(id);
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
