using System;
using System.Collections.Generic;
using System.Text;
using Administration.EventModel.Events;
using Infrastructure;

namespace Administration.Components.EventWriters
{
    public class RoomType:Writer
    {
	    private bool _active;
        public RoomType(
            Guid id,
			string name,
            string description)
        {
            if(id == Guid.Empty) throw new ArgumentException("bad Id", nameof(id));
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("empty name", nameof(name));
            if(string.IsNullOrWhiteSpace(description)) throw new ArgumentException("empty description", nameof(description));
            Raise(new RoomTypeAdded(id,name,description));
        }

        public void Deactivate()
        {
			if(!_active) return;
			Raise(new RoomTypeDeactivated(Id));
        }

        private void Apply(RoomTypeAdded evt)
        {
            Id = evt.TypeId;
            _active = true;
        }

        private void Apply(RoomTypeDeactivated evt)
        {
	        _active = false;
        }
    }
}
