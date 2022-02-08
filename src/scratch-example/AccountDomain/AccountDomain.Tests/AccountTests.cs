using System;
using Xunit;

namespace AccountDomain.Tests
{
    public class AccountTests
    {
        [Fact]
        public void can_create_accounts()
        {
            Assert.Throws<ArgumentException>(() => new AccountAggregate(Guid.Empty));
            var id = Guid.NewGuid();
            var account = new AccountAggregate(id);
            //test the id for the repo infrastructure
            Assert.Equal(id, account.Id);
            //the "real" test did we produce events?
            var @events = account.TakePendingEvents();

            Assert.Collection(
                          events,
                          e =>
                          {
                              var created = Assert.IsType<AccountMsgs.Created>(e);
                              Assert.Equal(id, created.Id);
                          });
        }
        [Fact]
        public void can_deposit_cash()
        {
            //event stream
            var created = new AccountMsgs.Created(Guid.NewGuid());
            
            //get empty aggregate
            var account = new AccountAggregate();
            
            //given
            account.Append(created); //hydrate aggregate with events
            //when
            var amount1 = 10;
            var amount2 = 15;
            account.DepositCash(amount1);
            account.DepositCash(amount2);
            //then
            var @events = account.TakePendingEvents();

            Assert.Collection(
                          events,
                          e =>
                          {
                              var deposit = Assert.IsType<AccountMsgs.CashDeposited>(e);
                              Assert.Equal(amount1, deposit.Amount);
                              Assert.Equal(created.Id, deposit.AccountId);
                          },
                           e =>
                           {
                               var deposit = Assert.IsType<AccountMsgs.CashDeposited>(e);
                               Assert.Equal(amount2, deposit.Amount);
                               Assert.Equal(created.Id, deposit.AccountId);
                           });

        }
        [Fact]
        public void can_withdraw_cash()
        {
            //event stream
            var created = new AccountMsgs.Created(Guid.NewGuid());
            var deposit10 = new AccountMsgs.CashDeposited(created.Id, 10);

            //get empty aggregate
            var account = new AccountAggregate();

            //given
            account.Append(created); //hydrate aggregate with events
            account.Append(deposit10);
            account.Append(deposit10);

            //when

            Assert.Throws<ArgumentException>(() => account.WithdrawCash(21));
            var @events = account.TakePendingEvents();
            Assert.Empty(events);

            var amount1 = 10;
            var amount2 = 5;
            account.WithdrawCash(amount1);
            account.WithdrawCash(amount2);

            //then
            @events = account.TakePendingEvents();

            Assert.Collection(
                          events,
                          e =>
                          {
                              var deposit = Assert.IsType<AccountMsgs.CashWithdrawn>(e);
                              Assert.Equal(amount1, deposit.Amount);
                              Assert.Equal(created.Id, deposit.AccountId);
                          },
                           e =>
                           {
                               var deposit = Assert.IsType<AccountMsgs.CashWithdrawn>(e);
                               Assert.Equal(amount2, deposit.Amount);
                               Assert.Equal(created.Id, deposit.AccountId);
                           });

        }
    }
}
