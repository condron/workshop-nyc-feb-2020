using AccountDomain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDomain
{
    public class AccountMsgs
    {
        public abstract class CreateAccount : ICommand { }
        public class CreateEmpty : CreateAccount
        {
            public readonly Guid Id;
            public readonly string DisplayName;
            public CreateEmpty(Guid id, string displayName)
            {
                Id = id;
                DisplayName = displayName;
            }
        }
        public class CreateWithCash : CreateAccount
        {
            public readonly Guid Id;
            public readonly string DisplayName;
            public readonly int OpeningBalance;
            public CreateWithCash(Guid id, string displayName, int openingBalance)
            {
                Id = id;
                DisplayName = displayName;
                OpeningBalance = openingBalance;
            }
        }
        public class Created:IEvent
        {
            public readonly Guid Id;
            public readonly string DisplayName;
            public Created(Guid id, string displayName)
            {
                Id = id;
                DisplayName = displayName;
            }
        }
        public abstract class Deposit : IEvent
        {
            public Guid AccountId { get; protected set; }
            public  int Amount { get; protected set; }
        }

        public class DepositCash : Deposit
        {           
            public DepositCash(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
        public class DepositCheck : Deposit
        {           
            public DepositCheck(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
        public class DepositPaycheck : Deposit
        {           
            public DepositPaycheck(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
        public class CashDeposited : IEvent
        {
            public readonly Guid AccountId;
            public readonly int Amount;
            public CashDeposited(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
        public class CashWithdrawn : IEvent
        {
            public readonly Guid AccountId;
            public readonly int Amount;
            public CashWithdrawn(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
        public class TransferFunds : ICommand {
            public readonly Guid TransferId;
            public readonly Guid SourceAccountId;
            public readonly Guid DestinationAccountId;
            public readonly int Amount;
            public TransferFunds(Guid transferId,Guid sourceAccountId, Guid destinationAccountId, int amount)
            {
                TransferId = transferId;
                SourceAccountId = sourceAccountId;
                DestinationAccountId = destinationAccountId;
                Amount = amount;
            }
        }
        public class TransferFundsReserved : IEvent
        {
            public readonly Guid TransferId;
            public readonly Guid SourceAccountId;
            public readonly Guid DestinationAccountId;
            public readonly int Amount;
            public TransferFundsReserved(Guid transferId, Guid sourceAccountId, Guid destinationAccountId, int amount)
            {
                TransferId = transferId;
                SourceAccountId = sourceAccountId;
                DestinationAccountId = destinationAccountId;
                Amount = amount;
            }
        }
        public class TransferFundsRecieved : IEvent
        {
            public readonly Guid TransferId;
            public readonly Guid SourceAccountId;
            public readonly Guid DestinationAccountId;
            public readonly int Amount;
            public TransferFundsRecieved(Guid transferId, Guid sourceAccountId, Guid destinationAccountId, int amount)
            {
                TransferId = transferId;
                SourceAccountId = sourceAccountId;
                DestinationAccountId = destinationAccountId;
                Amount = amount;
            }
        }
        public class TransferComplete : IEvent
        {
            public readonly Guid TransferId;
            public readonly Guid SourceAccountId;
            public readonly Guid DestinationAccountId;
            public TransferComplete(Guid transferId, Guid sourceAccountId, Guid destinationAccountId)
            {
                TransferId = transferId;
                SourceAccountId = sourceAccountId;
                DestinationAccountId = destinationAccountId;
            }
        }
        public class TransferCanceled : IEvent
        {
            public readonly Guid TransferId;
            public readonly Guid SourceAccountId;
            public readonly Guid DestinationAccountId;
            public TransferCanceled(Guid transferId, Guid sourceAccountId, Guid destinationAccountId)
            {
                TransferId = transferId;
                SourceAccountId = sourceAccountId;
                DestinationAccountId = destinationAccountId;
            }
        }
        public class OverdraftProtectionAdded : IEvent
        {
            public readonly Guid AccountId;
            public readonly int Amount;
            public OverdraftProtectionAdded(Guid accountId, int amount)
            {
                AccountId = accountId;
                Amount = amount;
            }
        }
        public class OverdraftAlert : IEvent
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
