using AccountDomain;
using System;
using System.Collections.Generic;

namespace AccountReadModels
{
    public class AccountList
    {
        public Dictionary<Guid, AccountDisplay> Accounts = new Dictionary<Guid, AccountDisplay>();
        public Dictionary<Guid, AccountDisplay> LargeAccounts = new Dictionary<Guid, AccountDisplay>();

        public void Handle(AccountMsgs.Created @event) {
            Accounts.Add(@event.Id, new AccountDisplay { Id = @event.Id });
        }
        public void Handle(AccountMsgs.CashDeposited @event) {
            if (Accounts.TryGetValue(@event.AccountId, out var account)) {
                account.Balance += @event.Amount;
                if (account.Balance >= 100 && !LargeAccounts.ContainsKey(@event.AccountId)) { 
                    LargeAccounts.Add(@event.AccountId, account);
                }
            }
        }
        public void Handle(AccountMsgs.CashWithdrawn @event){
            if (Accounts.TryGetValue(@event.AccountId, out var account))
            {
                account.Balance -= @event.Amount;
                if (account.Balance < 100 && !LargeAccounts.ContainsKey(@event.AccountId))
                {
                    LargeAccounts.Remove(@event.AccountId);
                }
            }
        }

    }
}
