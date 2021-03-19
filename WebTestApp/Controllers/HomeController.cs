using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebTestApp.Models;

namespace WebTestApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult AddAppeal(string mail, string phone, string text, int id)
        {
            //if (string.IsNullOrEmpty(mail))
            //{
            //    ModelState.AddModelError("mail", "Некорректное почта!");
            //}
            //else if (string.IsNullOrEmpty(phone))
            //{
            //    ModelState.AddModelError("phone", "Некорректный номер телефона!");
            //}
            //if (string.IsNullOrEmpty(text))
            //{
            //    ModelState.AddModelError("text", "Текст не должен быть пустым!");
            //}

            if (id != 0)
            {
                Appeal appeal = db.Appeals.FirstOrDefault(q => q.ID == id);
                if (appeal != null)
                    appeal.Text = text;
            }
            else
                db.Appeals.Add(new Appeal { Mail = mail, Phone = phone, Text = text });

            db.SaveChanges();

            ShowAppeals();

            return View("Appeals");
        }

        public IActionResult ShowAppeals()
        {
            ViewData["Data"] = db.Appeals.ToList();

            return View("Appeals");
        }

        public IActionResult DeleteAppeal(int id)
        {
            var appeal = db.Appeals.First(q => q.ID == id);
            db.Appeals.Remove(appeal);
            db.SaveChanges();

            ShowAppeals();

            return View("Appeals");
        }

        public IActionResult EditAppeal(int id)
        {
            var appeal = db.Appeals.First(q => q.ID == id);

            ViewData["mail"] = appeal.Mail;
            ViewData["phone"] = appeal.Phone;
            ViewData["text"] = appeal.Text;
            //ViewData["ID"] = appeal.ID;
            ViewBag.ID = appeal.ID;
            ViewBag.Mode = "readonly";

            return View("Index");
        }
    }
}
