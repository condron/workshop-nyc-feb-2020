using System;
using System.Collections.Generic;
using System.Text;
using Administration.EventModel.Events;
using Infrastructure;

namespace Administration.Components.EventWriters
{
    public class Room:Writer
    {
        public Room(
            Guid id,
			Guid typeId,
            string roomNumber)
        {
	        if(id == Guid.Empty) throw new ArgumentException("bad Id", nameof(id));
	        if(typeId == Guid.Empty) throw new ArgumentException("bad type Id", nameof(typeId));
            if(string.IsNullOrWhiteSpace(roomNumber)) throw new ArgumentException("empty room number", nameof(roomNumber));
            Raise(new RoomAdded(id,typeId,roomNumber));
        }
        private void Apply(RoomAdded evt)
        {
            Id = evt.TypeId;
        }
    }
}
