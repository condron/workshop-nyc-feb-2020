using AccountDomain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDomain.Services
{
    //application service
    public class AccountSvc :
        IHandleCommand<AccountMsgs.CreateEmpty>,
        IHandleCommand<AccountMsgs.CreateWithCash>,
        IHandleCommand<AccountMsgs.DepositCash>

    {
        private readonly IRepository _repo;
        private readonly ITransferService _transferService;
        public AccountSvc(IRepository repo)
        {
            _repo = repo;
            _transferService = new TransferService(repo);
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
            try
            {  
                _transferService.Transfer(cmd.TransferId, cmd.SourceAccountId, cmd.DestinationAccountId, cmd.Amount);
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
