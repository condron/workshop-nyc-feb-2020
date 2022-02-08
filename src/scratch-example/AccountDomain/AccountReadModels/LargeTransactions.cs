using AccountDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountReadModels
{
    public class LargeTransactionCount
    {
        //accountId , Count of Large transactions
        public Dictionary<Guid, int> Accounts = new Dictionary<Guid, int>();

        public void Handle(AccountMsgs.CashDeposited @event)
        {
            if (@event.Amount > 10_000) { LogTransaction(@event.AccountId, @event.Amount); }
        }
        public void Handle(AccountMsgs.CashWithdrawn @event)
        {
            if (@event.Amount > 10_000) { LogTransaction(@event.AccountId, @event.Amount); }
           
        }
        private void LogTransaction(Guid id, int amount) {
            if (Accounts.ContainsKey(id))
            {
                Accounts[id]++;
            }
            else
            {
                Accounts.Add(id, 1);
            }
        }

    }
}
