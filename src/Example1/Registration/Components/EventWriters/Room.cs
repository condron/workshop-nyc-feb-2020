using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure;
using Registration.Blueprint.Events;

namespace Registration.Components.EventWriters
{
    public class Room:Writer
    {
        public Room(
            Guid roomId,
            string roomNumber,
            string roomLocation,
            string roomType)
        {
            if (roomId == Guid.Empty) {
                throw  new ArgumentNullException(nameof(roomId));
            }
            if (string.IsNullOrWhiteSpace(roomNumber)) {
                throw new ArgumentNullException(nameof(roomNumber));
            }
            if (string.IsNullOrWhiteSpace(roomLocation)) {
                throw new ArgumentNullException(nameof(roomLocation));
            }
            if (string.IsNullOrWhiteSpace(roomType)) {
                throw new ArgumentNullException(nameof(roomType));
            }

            Raise(new RoomAdded( roomId,roomNumber, roomLocation, roomType));
        }

        private void Apply(RoomAdded evt)
        {
            Id = evt.RoomId;
        }
    }
}
