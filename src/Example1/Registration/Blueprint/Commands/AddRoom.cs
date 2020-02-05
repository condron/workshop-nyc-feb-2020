using System;
using Infrastructure;

namespace Registration.Blueprint.Commands
{
    public class AddRoom : ICommand
    {
        public readonly Guid RoomId;
        public readonly string RoomNumber;
        public readonly string RoomLocation;
        public readonly string RoomType;

        public AddRoom(
            Guid roomId,
            string roomNumber,
            string roomLocation,
            string roomType
        )
        {
            RoomId = roomId;
            RoomNumber = roomNumber;
            RoomLocation = roomLocation;
            RoomType = roomType;
        }
    }
}
