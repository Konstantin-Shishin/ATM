using ATMApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ATMApp.Controllers
{
    public class HomeController : Controller
    {
        ATM ATM = new ATM();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(ATM);
        }
        [HttpPost]
        public IActionResult GiveOut(int GiveOutSum, int Nominal)
        {
            //нормальный случай
            ATM.bills[Nominal] = ATM.bills[Nominal] - GiveOutSum / Nominal;
            return View("Index", ATM);
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
    }
}
