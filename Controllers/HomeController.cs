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

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Index", InfoATM.ATM);
        }

        [HttpPost]
        public IActionResult GiveOut(int? GiveOutSum, int? NominalGiv)
        {
            

            if (GiveOutSum == null || NominalGiv == null)
            {
                return BadRequest("Не указаны параметры запроса");
            }

            else if (NominalGiv > GiveOutSum)
            {
                return BadRequest("Номинал купюры больше требуемой суммы к выдаче, вернитесь назад");
            }

            // запрошенная сумма с единицами (неверный формат)
            else if (GiveOutSum % 10 != 0)
            {
                return BadRequest("Невозможно произвести выдачу номиналом менее 10Р");
            }
            else
            {
                string result = InfoATM.ATM.GiveOut(GiveOutSum.Value, NominalGiv.Value);
                if (result != "GiveOut")
                {
                    return BadRequest(result);
                }
                else
                {
                    return View("Index", InfoATM.ATM);
                }                
            }
        }

        [HttpPost]
        public IActionResult Receive(int? ReceiveSum, int? NominalRec)
        {
            // не заполнены поля
            if (ReceiveSum == null || NominalRec == null)
            {
                return BadRequest("Не указаны параметры запроса");
            }
            else if (ReceiveSum % 10 != 0)
            {
                return BadRequest("Невозможно произвести внесение номиналом менее 10Р");
            }
            else
            {
                string result = InfoATM.ATM.Receive(ReceiveSum.Value, NominalRec.Value);
                if (result != "Receive")
                {
                    return BadRequest(result);
                }
                else
                {
                    return View("Index", InfoATM.ATM);
                }
            }


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
