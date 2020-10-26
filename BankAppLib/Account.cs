using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BankAppLib
{
    public abstract class Account
    {
        public abstract int Amount { get; set; }
        public Account(Guid guid)
        {
            
        }
        public abstract bool Deposit(int account);
        public abstract bool Withdraw(int amount);
    }
}