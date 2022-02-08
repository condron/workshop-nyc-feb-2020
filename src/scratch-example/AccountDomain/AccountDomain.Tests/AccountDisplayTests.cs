using AccountReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AccountDomain.Tests
{
    public  class AccountDisplayTests
    {
        [Fact]
        public void can_display_accounts() {
            //given
            var created1 = new AccountMsgs.Created(Guid.NewGuid());
            var created2 = new AccountMsgs.Created(Guid.NewGuid());
            var created3 = new AccountMsgs.Created(Guid.NewGuid());

            var list = new AccountList();
            //when
            list.Handle(created1);
            list.Handle(created2);
            list.Handle(created3);

            //then

            var readmodel = list.Accounts.Values.ToArray();
            Assert.Collection(readmodel,
                                acct => Assert.Equal(created1.Id, acct.Id),
                                acct => Assert.Equal(created2.Id, acct.Id),
                                acct => Assert.Equal(created3.Id, acct.Id));
        }
        [Fact]
        public void can_display_account_balances()
        {
            //given
            var created1 = new AccountMsgs.Created(Guid.NewGuid());
            var created2 = new AccountMsgs.Created(Guid.NewGuid());
            var created3 = new AccountMsgs.Created(Guid.NewGuid());
            var USD10 = 10;
            var USD15 = 15;
            var USD16 = 16;

            var list = new AccountList();
            //when
            list.Handle(created1);
            list.Handle(new AccountMsgs.CashDeposited(created1.Id, USD10));
            //balance = 10
            list.Handle(created2);
            list.Handle(new AccountMsgs.CashWithdrawn(created2.Id, USD16));
            //balance = -16
            list.Handle(created3);
            list.Handle(new AccountMsgs.CashDeposited(created3.Id, USD16));
            list.Handle(new AccountMsgs.CashWithdrawn(created3.Id, USD15));
            // balance = 1
            //then

            var readmodel = list.Accounts.Values.ToArray();
            Assert.Collection(readmodel,
                                acct => Assert.Equal(10, acct.Balance),
                                acct => Assert.Equal(-16, acct.Balance),
                                acct => Assert.Equal(1, acct.Balance));
        }
    }
}
