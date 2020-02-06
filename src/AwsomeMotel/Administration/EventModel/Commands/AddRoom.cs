using System;
using Infrastructure.Interfaces;

namespace Administration.EventModel.Commands
{
    public class AddRoom:ICommand
    {
	    public readonly Guid RoomId;
	    public readonly Guid TypeId;
        public readonly string RoomNumber;

        public AddRoom(
	        Guid roomId,
	        Guid typeId,
			string roomNumber)
        {
	        RoomId = roomId;
	        TypeId = typeId;
            RoomNumber = roomNumber;
        }
    }
}
