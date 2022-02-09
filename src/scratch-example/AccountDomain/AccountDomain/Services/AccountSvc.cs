using AccountDomain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDomain.Services
{
    public class AccountSvc :
        IHandleCommand<AccountMsgs.CreateEmpty>,
        IHandleCommand<AccountMsgs.CreateWithCash>,
        IHandleCommand<AccountMsgs.DepositCash>

    {
        private readonly IRepository _repo;

        public AccountSvc(IRepository repo)
        {
            _repo = repo;
        }
        public bool Handle(AccountMsgs.CreateEmpty cmd)
        {
            var account = new AccountAggregate(cmd.Id, cmd.DisplayName);
            _repo.Save(account);
            return true;
        }
        public bool Handle(AccountMsgs.CreateWithCash cmd)
        {
            var account = new AccountAggregate(cmd.Id, cmd.DisplayName);
            account.DepositCash(cmd.OpeningBalance);
            _repo.Save(account);
            return true;
        }
        public bool Handle(AccountMsgs.DepositCash cmd)
        {
            var account = _repo.GetbyId<AccountAggregate>(cmd.AccountId);
            account.DepositCash(cmd.Amount);
            _repo.Save(account);
            return true;
        }
        public bool Handle(AccountMsgs.TransferFunds cmd)
        {
            AccountAggregate source;
            //reserve funds
            try
            {
                source = _repo.GetbyId<AccountAggregate>(cmd.SourceAccountId);
                source.ReserveFunds(cmd.TransferId, cmd.DestinationAccountId, cmd.Amount);
                _repo.Save(source);
            }
            catch (Exception _)
            {
                //todo: tell the caller unable to reserve funds
                return false;
            }
            //send funds
            try
            {
                var destination = _repo.GetbyId<AccountAggregate>(cmd.DestinationAccountId);
                destination.RecieveFunds(cmd.TransferId, cmd.SourceAccountId, cmd.Amount);
                _repo.Save(destination);
            }
            catch (Exception _)
            {
                //todo: tell the caller unable to reserve funds
                try {
                    source.CancelTransfer(cmd.TransferId);
                    _repo.Save(source);
                }
                catch 
                {
                    //todo: tell the caller unable to cancel transaction
                   //shouldn't happen
                }
                return false;
            }
            //complete transaction
            try
            {                
                source.CompleteTransfer(cmd.TransferId);
                _repo.Save(source);
            }
            catch (Exception _)
            {
                //todo: tell the caller unable to reserve funds
                return false;
            }
            return true;
        }
    }
}
