using System;
using System.Collections.Generic;
using System.Text;
using Administration.EventModel.Events;
using Infrastructure;

namespace Administration.Components.EventWriters
{
    public class RoomType:Writer
    {
        public RoomType(
            Guid id,
            string description)
        {
            if(id == Guid.Empty) throw new ArgumentException("bad Id", nameof(id));
            if(string.IsNullOrWhiteSpace(description)) throw new ArgumentException("empty description", nameof(description));
            Raise(new RoomTypeAdded(id,description));
        }

        private void Apply(RoomTypeAdded evt)
        {
            Id = evt.TypeId;
        }
    }
}
