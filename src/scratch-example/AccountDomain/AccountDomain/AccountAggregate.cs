using System;
using System.Collections.Generic;

namespace AccountDomain
{
    public class AccountAggregate
    {
        private List<object> _pendingEvents = new List<object>();
        private int _balance;
        //only public for the repository 
        public Guid Id { get; private set; }

        //infrastructre use only!!!!
        public AccountAggregate() { }

        //public behavoirs
        public AccountAggregate(Guid id)
        {
            if (id == Guid.Empty) { throw new ArgumentException("Empty ID is not allowed!"); }
            Raise(new AccountMsgs.Created(id));
        }

        public void DepositCash(int amount)
        {
            if (amount <= 0) { throw new ArgumentOutOfRangeException("Can't deposit 0 or negative money"); }
            Raise(new AccountMsgs.CashDeposited(Id, amount));
        }
        public void WithdrawCash(int amount)
        {
            if (amount <= 0) { throw new ArgumentOutOfRangeException("Can't withdraw 0 or negative money"); }
            if( _balance - amount < 0) { throw new ArgumentException("Overdraft!!!"); }
            Raise(new AccountMsgs.CashWithdrawn(Id, amount));
        }

        //apply methods
        private void Apply(AccountMsgs.Created @event)
        {
            Id = @event.Id;
        }
        private void Apply(AccountMsgs.CashDeposited @event)
        {
            _balance += @event.Amount;
        }
        private void Apply(AccountMsgs.CashWithdrawn @event)
        {
            _balance -= @event.Amount;
        }

        //todo: move into base class and use reflection or something
        private void Raise(object @event)
        {
            _pendingEvents.Add(@event);
            Append(@event);
        }
        public void Append(object @event)
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
        }

        public List<object> TakePendingEvents()
        {
            var @events = _pendingEvents;
            _pendingEvents = new List<object>();
            return @events;
        }
    }
}
