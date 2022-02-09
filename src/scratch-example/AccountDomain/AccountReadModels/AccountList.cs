using AccountDomain;
using System;
using System.Collections.Generic;

namespace AccountReadModels
{
    public class AccountList
    {
        public Dictionary<Guid, AccountDisplayDTO> Accounts = new Dictionary<Guid, AccountDisplayDTO>();
        public Dictionary<Guid, AccountDisplayDTO> LargeAccounts = new Dictionary<Guid, AccountDisplayDTO>();

        public void Handle(AccountMsgs.Created @event) {
            Accounts.Add(@event.Id, new AccountDisplayDTO { Id = @event.Id });
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
