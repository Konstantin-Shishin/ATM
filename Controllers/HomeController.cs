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

            if (NominalGiv > GiveOutSum)
            {
                return BadRequest("Номинал купюры больше требуемой суммы к выдаче, вернитесь назад");
            }

            // недостаточно средств для выдачи
            if (InfoATM.ATM.bills.Keys.Select(a => a * InfoATM.ATM.bills[a]).Sum()<GiveOutSum)
            {
                return BadRequest("Недостаточно средств, вернитесь назад");
            }

            // запрошенная сумма с единицами (неверный формат)
            if (GiveOutSum % 10 != 0)
            {
                return BadRequest("Невозможно произвести выдачу номиналом менее 10Р");
            }

            // попытка выдачи
            int remain = (GiveOutSum % NominalGiv).Value;
            if (remain == 0) // выдача полностью запрошенныи номиналом
            {
                if (InfoATM.ATM.bills[NominalGiv.Value] - (GiveOutSum / NominalGiv).Value < 0)
                {
                    return BadRequest("Невозможно произвести выдачу только этим номиналом, недостаточно купюр. Вернитесь назад");
                }
                InfoATM.ATM.bills[NominalGiv.Value] = InfoATM.ATM.bills[NominalGiv.Value] - (GiveOutSum / NominalGiv).Value;
                return View("Index", InfoATM.ATM);
            }
            else
            {
                // выдача номиналом заданным, максимально возможная
                InfoATM.ATM.bills[NominalGiv.Value] = InfoATM.ATM.bills[NominalGiv.Value] - (GiveOutSum / NominalGiv).Value; 
                // выдача остатка
                InfoATM.ATM.GiveOutAlgorithm(remain);
                return View("Index", InfoATM.ATM);
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
            //ограничение по количеству хранимых купюр
            if ((InfoATM.ATM.bills.Values.Select(a => a).Sum() + (ReceiveSum/NominalRec).Value) > InfoATM.ATM.CountMaxBills)
            {
                return BadRequest("Превышение лимита количества купюр, вернитесь назад");
            }

            // норм
            InfoATM.ATM.bills[NominalRec.Value] = InfoATM.ATM.bills[NominalRec.Value] + (ReceiveSum / NominalRec).Value;
            return View("Index", InfoATM.ATM);
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
