using System;
using Infrastructure;
using Registration.Blueprint.Commands;
using Registration.Components.EventWriters;

namespace Registration.Components.CommandHandlers
{
    public class RoomSvc:
        IHandleCommand<AddRoom>
    {
        private readonly IRepository _repository;

        public RoomSvc(IRepository repository)
        {
            _repository = repository;
        }

        public bool Handle(AddRoom cmd)
        {
            try {
               var room = new Room(
                   cmd.RoomId,
                   cmd.RoomNumber,
                   cmd.RoomLocation,
                   cmd.RoomType);
               _repository.Save(room);
               return true;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
