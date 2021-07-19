using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATMApp.Models;


namespace ATMApp.Models
{
    public class ATM
    {
        public int CountMaxBills = 10000;
        public Dictionary<int, int> bills;

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
        }

        public void GetBills()
        {
            return;
        }
        public void GiveOutBills()
        {
            return;
        }

        // выдача остатка
        public void GiveOutAlgorithm(int remain)
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
