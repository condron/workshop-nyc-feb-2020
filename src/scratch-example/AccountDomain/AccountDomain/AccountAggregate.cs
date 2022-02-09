using AccountDomain.Infrastructure;
using System;
using System.Collections.Generic;

namespace AccountDomain
{

    public class AccountAggregate: IEventDrivenStateMachine
    {
        private List<IEvent> _pendingEvents = new List<IEvent>();
        private int _availableBalance;
        private int _balance;
        //transferID, destination account, transfer amount
        private Dictionary<Guid, Tuple<Guid, int>> _openTransfers = new Dictionary<Guid, Tuple<Guid, int>>();
        //only public for the repository 
        public Guid Id { get; private set; }
        public string Name => GetType().Name;
        //infrastructre use only!!!!
        public AccountAggregate() { }

        //public behavoirs
        public AccountAggregate(Guid id, string displayName)
        {
            if (id == Guid.Empty) { throw new ArgumentException("Empty ID is not allowed!"); }
            if (string.IsNullOrWhiteSpace(displayName)) { throw new ArgumentException("Empty displayName is not allowed!"); }
            Raise(new AccountMsgs.Created(id, displayName));
        }

        public void DepositCash(int amount)
        {
            if (amount <= 0) { throw new ArgumentOutOfRangeException("Can't deposit 0 or negative money"); }
            Raise(new AccountMsgs.CashDeposited(Id, amount));
        }
        public void WithdrawCash(int amount)
        {
            if (amount <= 0) { throw new ArgumentOutOfRangeException("Can't withdraw 0 or negative money"); }
            if(_availableBalance - amount < 0) { throw new ArgumentException("Overdraft!!!"); }
            if (_balance - amount < 0) {
                Raise(new AccountMsgs.OverdraftAlert(Id, amount));
            }
            Raise(new AccountMsgs.CashWithdrawn(Id, amount));
        }
        public void AddOverDraftProtection(int amount) {
            if (amount <= 0) { throw new ArgumentOutOfRangeException("Can't add overdraft of 0 or negative money"); }
            Raise(new AccountMsgs.OverdraftProtectionAdded(Id, amount));
        }

        public void ReserveFunds(Guid transferId, Guid destination, int amount) {
            //todo : validate everything
            if (_availableBalance - amount < 0) { 
                //todo: raise failed transaction event for the recon process
                throw new ArgumentException("Unable to reserve funds!!!"); 
            }
            Raise(new AccountMsgs.TransferFundsReserved(transferId, Id, destination, amount));
        }
        public void RecieveFunds(Guid transferId, Guid source, int amount)
        {
            //todo : validate everything
            Raise(new AccountMsgs.TransferFundsRecieved(transferId, source, Id, amount));
        }
        public void CompleteTransfer(Guid transferId)
        {
            //todo : validate everything
            if (!_openTransfers.ContainsKey(transferId)) { throw new ArgumentException("Unknown transfer!!!"); }
            Raise(new AccountMsgs.TransferComplete(transferId, Id, _openTransfers[transferId].Item1));
        }
        public void CancelTransfer(Guid transferId)
        {
            //todo : validate everything
            if (!_openTransfers.ContainsKey(transferId)) { throw new ArgumentException("Unknown transfer!!!"); }
            Raise(new AccountMsgs.TransferCanceled(transferId, Id, _openTransfers[transferId].Item1));
        }
        //apply methods
        private void Apply(AccountMsgs.Created @event)
        {
            Id = @event.Id;
        }
        private void Apply(AccountMsgs.CashDeposited @event)
        {
            _availableBalance += @event.Amount;
            _balance += @event.Amount;
        }
        private void Apply(AccountMsgs.CashWithdrawn @event)
        {
            _availableBalance -= @event.Amount;
            _balance -= @event.Amount;
        }
        private void Apply(AccountMsgs.OverdraftProtectionAdded @event)
        {
            _availableBalance += @event.Amount;
        }
        private void Apply(AccountMsgs.TransferFundsReserved @event)
        {
            _openTransfers.Add(@event.TransferId, new Tuple<Guid, int>(@event.DestinationAccountId, @event.Amount));
            _availableBalance -= @event.Amount;
        }
        private void Apply(AccountMsgs.TransferFundsRecieved @event)
        {
            _availableBalance += @event.Amount;
        }
        private void Apply(AccountMsgs.TransferComplete @event)
        {
            _openTransfers.Remove(@event.TransferId);
        }
        private void Apply(AccountMsgs.TransferCanceled @event)
        {
            var amount = _openTransfers[@event.TransferId].Item2;
            _availableBalance += amount;
            _openTransfers.Remove(@event.TransferId);
        }
        //todo: move into base class and use reflection or something
        private void Raise(IEvent @event)
        {
            _pendingEvents.Add(@event);
            Apply(@event);
        }
        public void Apply(IEvent @event)
        {
            if (@event.GetType() == typeof(AccountMsgs.Created))
            {
                Apply((AccountMsgs.Created)@event);
                return;
            }
            if (@event.GetType() == typeof(AccountMsgs.CashWithdrawn))
            {
                Apply((AccountMsgs.CashWithdrawn)@event);
                return;
            }
            if (@event.GetType() == typeof(AccountMsgs.CashDeposited))
            {
                Apply((AccountMsgs.CashDeposited)@event);
                return;
            }
            if (@event.GetType() == typeof(AccountMsgs.OverdraftProtectionAdded))
            {
                Apply((AccountMsgs.OverdraftProtectionAdded)@event);
                return;
            }
            if (@event.GetType() == typeof(AccountMsgs.TransferFundsReserved))
            {
                Apply((AccountMsgs.TransferFundsReserved)@event);
                return;
            }
            if (@event.GetType() == typeof(AccountMsgs.TransferFundsRecieved))
            {
                Apply((AccountMsgs.TransferFundsRecieved)@event);
                return;
            }
            if (@event.GetType() == typeof(AccountMsgs.TransferComplete))
            {
                Apply((AccountMsgs.TransferComplete)@event);
                return;
            }
            if (@event.GetType() == typeof(AccountMsgs.TransferCanceled))
            {
                Apply((AccountMsgs.TransferCanceled)@event);
                return;
            }
        }

        public List<IEvent> TakeEvents()
        {
            var @events = _pendingEvents;
            _pendingEvents = new List<IEvent>();
            return @events;
        }
    }
}
