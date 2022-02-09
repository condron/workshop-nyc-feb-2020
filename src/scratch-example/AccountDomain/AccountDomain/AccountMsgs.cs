using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDomain
{
    public class AccountMsgs
    {
        public class Created
        {
            public readonly Guid Id;
            public readonly string DisplayName;
            public Created(Guid id, string displayName)
            {
                Id = id;
                DisplayName = displayName;
            }

            
        }
        public class CashDeposited
        {
            public readonly Guid AccountId;
            public readonly int Amount;
            public CashDeposited(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
        public class CashWithdrawn
        {
            public readonly Guid AccountId;
            public readonly int Amount;
            public CashWithdrawn(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
        public class OverdraftProtectionAdded
        {
            public readonly Guid AccountId;
            public readonly int Amount;
            public OverdraftProtectionAdded(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
        public class OverdraftAlert
        {
            public readonly Guid AccountId;
            public readonly int Amount;
            //todo: add transaction reference to actual withdrawl
            public OverdraftAlert(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
    }
}
