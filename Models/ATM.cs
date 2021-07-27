using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATMApp.Models;


namespace ATMApp.Models
{
    public class ATM
    {
        private int CountMaxBills { get; set; }
        private Dictionary<int, int> bills { get; set; }

        public ATM()
        {
            bills = new Dictionary<int, int>(7);
            bills.Add(10, 1000);
            bills.Add(50, 1000);
            bills.Add(100, 1000);
            bills.Add(200, 1000);
            bills.Add(500, 1000);
            bills.Add(1000, 500);
            bills.Add(5000, 500);
            CountMaxBills = 10000;
        }

        private bool InvariantNominal(int Nominal) // Инвариант наличия записи о купюрах заданного номинала
        {
            if (!bills.ContainsKey(Nominal))
            {
                return false;
            }
            else
            {
                return true;
            }           
        }

        public string GiveOut(int GiveOutSum, int NominalGiv)
        {

            if (!InvariantNominal(NominalGiv))
            {
                return "Недопустимая купюра";
            }

            // недостаточно средств для выдачи
            if (InfoATM.ATM.GetDictBills().Keys.Select(a => a * InfoATM.ATM.GetDictBills()[a]).Sum() < GiveOutSum)
            {
                return "Недостаточно средств, вернитесь назад";
            }

            // попытка выдачи
            int remain = (GiveOutSum % NominalGiv); 
            if (remain == 0) // выдача полностью запрошенныи номиналом
            {
                if (InfoATM.ATM.GetDictBills()[NominalGiv] - (GiveOutSum / NominalGiv) < 0)
                {
                    return "Невозможно произвести выдачу только этим номиналом, недостаточно купюр. Вернитесь назад";
                }
                InfoATM.ATM.GetDictBills()[NominalGiv] = InfoATM.ATM.GetDictBills()[NominalGiv] - (GiveOutSum / NominalGiv);
                return "GiveOut";
            }
            else
            {
                if (InfoATM.ATM.GetDictBills()[NominalGiv] - (GiveOutSum / NominalGiv) < 0)
                {
                    return "Невозможно произвести выдачу только этим номиналом, недостаточно купюр. Вернитесь назад";
                }
                // выдача номиналом заданным, максимально возможная
                InfoATM.ATM.GetDictBills()[NominalGiv] = InfoATM.ATM.GetDictBills()[NominalGiv] - (GiveOutSum / NominalGiv);
                // выдача остатка
                InfoATM.ATM.GiveOutAlgorithm(remain);
            }
            return "GiveOut";
        }

        public string Receive(int ReceiveSum, int NominalRec)
        {
            if (!InvariantNominal(NominalRec))
            {
                return "Недопустимая купюра";
            }

            else if (ReceiveSum <= 0)
            {
                return "Сумма меньше либо равна 0";
            }
            
            //ограничение по количеству хранимых купюр
            else if ((InfoATM.ATM.GetDictBills().Values.Select(a => a).Sum() + (ReceiveSum / NominalRec)) > InfoATM.ATM.CountMaxBills)
            {
                return "Превышение лимита количества купюр, вернитесь назад";
            }

            // норм
            InfoATM.ATM.GetDictBills()[NominalRec] = InfoATM.ATM.GetDictBills()[NominalRec] + (ReceiveSum / NominalRec);
            return "Receive";
        }
        public Dictionary<int, int> GetDictBills()
        {
            return bills;
        }

        // выдача остатка
        private void GiveOutAlgorithm(int remain)
        {
            int max_key = InfoATM.ATM.bills.Keys.Select(a => a).Where(a => a <= remain).Max();

            // например, остаток равен 1000 (выдаем 1 купюру номиналом 1000)
            if (max_key == remain)
            {
                InfoATM.ATM.bills[max_key] = InfoATM.ATM.bills[max_key] - 1;
                return;
            }
            // например, остаток равен 3200 (выдаем 3 купюры номиналом 1000 и 1 купюру номиналом 200)
            else
            {
                int n_bills = remain / max_key; // целая часть от деления
                InfoATM.ATM.bills[max_key] = InfoATM.ATM.bills[max_key] - n_bills; // выдали часть остатка максимально возможными купюрами
                remain = remain % max_key; // осталось выдать 200
                if (remain == 0) return;
                InfoATM.ATM.GiveOutAlgorithm(remain);
            }
        }
    }
}
