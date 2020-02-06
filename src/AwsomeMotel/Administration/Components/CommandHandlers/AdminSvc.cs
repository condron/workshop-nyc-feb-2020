using System;
using System.Collections.Generic;
using System.Text;
using Administration.Components.EventWriters;
using Administration.EventModel.Commands;
using Infrastructure.Interfaces;

namespace Administration.Components.CommandHandlers
{
    public class AdminSvc:
	    IHandleCommand<AddRoomType>,
	    IHandleCommand<DeactivateRoomType>
    {
        private readonly IRepository _repo;

        public AdminSvc(IRepository repo)
        {
            _repo = repo;
        }
        public bool Handle(AddRoomType cmd)
        {
            try {
                var roomType = new RoomType(cmd.TypeId, cmd.Name, cmd.Description);
                _repo.Save(roomType);
            }
            catch (Exception _) {
                return false;
            }

            return true;
        }

        public bool Handle(DeactivateRoomType cmd)
        {
	        try {
		        var roomType = _repo.Load<RoomType>(cmd.TypeId);
				roomType.Deactivate();
		        _repo.Save(roomType);
	        }
	        catch (Exception _) {
		        return false;
	        }

	        return true;
        }
    }
}
