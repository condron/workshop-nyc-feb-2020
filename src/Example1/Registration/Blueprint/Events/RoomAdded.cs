using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure;

namespace Registration.Blueprint.Events
{
    public class RoomAdded : IEvent
    {
        public readonly Guid RoomId;
        public readonly string RoomNumber;
        public readonly string RoomLocation;
        public readonly string RoomType;

        public RoomAdded(
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
