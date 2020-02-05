using System;
using Infrastructure.Interfaces;


namespace Administration.Blueprint.Events
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
