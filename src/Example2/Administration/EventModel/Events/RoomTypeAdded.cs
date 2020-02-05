using System;
using Infrastructure.Interfaces;

namespace Administration.EventModel.Events
{
    public class RoomTypeAdded:IEvent
    {
        public readonly Guid TypeId;
        public readonly string Description;

        public RoomTypeAdded(
            Guid typeId,
            string description)
        {
            TypeId = typeId;
            Description = description;
        }
    }
}
