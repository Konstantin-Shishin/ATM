using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATMApp.Models;


namespace ATMApp.Models
{
    public class ATM
    {
        public int CountMaxBills = 1000;
        public Dictionary<int, int> bills;

        public ATM()
        {
            bills = new Dictionary<int, int>(7);
            bills.Add(10,101);
            bills.Add(50, 100);
            bills.Add(100, 100);
            bills.Add(200, 100);
            bills.Add(500, 100);
            bills.Add(1000, 100);
            bills.Add(5000, 100);
        }

        public void GetBills()
        {
            return;
        }
        public void GiveOutBills()
        {
            return;
        }



    }
}
