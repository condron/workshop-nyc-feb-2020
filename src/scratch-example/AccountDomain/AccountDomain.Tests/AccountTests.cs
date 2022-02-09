using System;
using Xunit;

namespace AccountDomain.Tests
{
    public class AccountTests
    {
        private string displayName = "testName";
        [Fact]
        public void can_create_accounts()
        {
            Assert.Throws<ArgumentException>(() => new AccountAggregate(Guid.Empty, displayName));
            Assert.Throws<ArgumentException>(() => new AccountAggregate(Guid.Empty, ""));
            Assert.Throws<ArgumentException>(() => new AccountAggregate(Guid.Empty, null));
            Assert.Throws<ArgumentException>(() => new AccountAggregate(Guid.Empty, "/t"));

            var id = Guid.NewGuid();
            var account = new AccountAggregate(id,displayName);
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
                              Assert.Equal(displayName, created.DisplayName);
                          });
        }
        [Fact]
        public void can_deposit_cash()
        {
            //event stream
           
            var created = new AccountMsgs.Created(Guid.NewGuid(), displayName);
            
            //get empty aggregate
            var account = new AccountAggregate();
            
            //given
            account.Apply(created); //hydrate aggregate with events
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
            var created = new AccountMsgs.Created(Guid.NewGuid(), displayName);
            var deposit10 = new AccountMsgs.CashDeposited(created.Id, 10);

            //get empty aggregate
            var account = new AccountAggregate();

            //given
            account.Apply(created); //hydrate aggregate with events
            account.Apply(deposit10);
            account.Apply(deposit10);

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
        [Fact]
        public void can_overdraw_cash()
        {
            //event stream
            var created = new AccountMsgs.Created(Guid.NewGuid(), displayName);
            var deposit10 = new AccountMsgs.CashDeposited(created.Id, 10);
            var overDraft20 = new AccountMsgs.OverdraftProtectionAdded(created.Id, 20);

            //get empty aggregate
            var account = new AccountAggregate();

            //given
            account.Apply(created); //hydrate aggregate with events
            account.Apply(deposit10);
            account.Apply(deposit10);
            account.Apply(overDraft20);


            //when ( available balance == 40)

            //not allowed
            Assert.Throws<ArgumentException>(() => account.WithdrawCash(41));
            //no events added
            var @events = account.TakePendingEvents();
            Assert.Empty(events);


            var amount10 = 10;
            var amount5 = 5;
            
            account.WithdrawCash(amount10);
            account.WithdrawCash(amount5);
            account.AddOverDraftProtection(amount10);
            account.WithdrawCash(amount10);

            //then
            @events = account.TakePendingEvents();

            Assert.Collection(
                          events,
                          e =>
                          {
                              var deposit = Assert.IsType<AccountMsgs.CashWithdrawn>(e);
                              Assert.Equal(amount10, deposit.Amount);
                              Assert.Equal(created.Id, deposit.AccountId);
                          },
                           e =>
                           {
                               var deposit = Assert.IsType<AccountMsgs.CashWithdrawn>(e);
                               Assert.Equal(amount5, deposit.Amount);
                               Assert.Equal(created.Id, deposit.AccountId);
                           },
                           e =>
                           {
                               var overdraft = Assert.IsType<AccountMsgs.OverdraftProtectionAdded>(e);
                               Assert.Equal(amount10, overdraft.Amount);
                               Assert.Equal(created.Id, overdraft.AccountId);
                           },
                           e =>
                           {
                               var deposit = Assert.IsType<AccountMsgs.OverdraftAlert>(e);
                               Assert.Equal(amount10, deposit.Amount);
                               Assert.Equal(created.Id, deposit.AccountId);
                           },
                           e =>
                           {
                               var deposit = Assert.IsType<AccountMsgs.CashWithdrawn>(e);
                               Assert.Equal(amount10, deposit.Amount);
                               Assert.Equal(created.Id, deposit.AccountId);
                           });

        }
    }
}
