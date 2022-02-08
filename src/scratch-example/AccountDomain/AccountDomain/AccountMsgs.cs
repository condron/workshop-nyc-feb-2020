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
            public Created(Guid id)
            {
                Id = id;
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
    }
}
