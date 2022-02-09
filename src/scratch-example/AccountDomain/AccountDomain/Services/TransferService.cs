using AccountDomain.Infrastructure;
using System;

namespace AccountDomain.Services
{
    //domain service
    public interface ITransferService { void Transfer(Guid transferId, Guid sourceId, Guid destinationId, int amount); }
    public class TransferService : ITransferService
    {
        private readonly IRepository _repo;

        public TransferService(IRepository repo)
        {
            _repo = repo;
        }
        public void Transfer(Guid transferId, Guid sourceId, Guid destinationId, int amount)
        {

            AccountAggregate source;
            //reserve funds
            try
            {
                source = _repo.GetbyId<AccountAggregate>(sourceId);
                source.ReserveFunds(transferId, destinationId, amount);
                _repo.Save(source);
            }
            catch (Exception _)
            {
                //todo: tell the caller unable to reserve funds
                throw;
            }
            //send funds
            try
            {
                var destination = _repo.GetbyId<AccountAggregate>(destinationId);
                destination.RecieveFunds(transferId, sourceId, amount);
                _repo.Save(destination);
            }
            catch (Exception _)
            {
                //todo: tell the caller unable to reserve funds
                try
                {
                    source.CancelTransfer(transferId);
                    _repo.Save(source);
                }
                catch
                {
                    throw;
                }
                throw;
            }
            //complete transaction
            try
            {
                source.CompleteTransfer(transferId);
                _repo.Save(source);
            }
            catch (Exception _)
            {
                throw;
            }

        }
    }
}
