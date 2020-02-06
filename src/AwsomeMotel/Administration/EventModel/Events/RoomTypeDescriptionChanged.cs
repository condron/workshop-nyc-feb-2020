using System;
using Infrastructure.Interfaces;

namespace Administration.EventModel.Events
{
    public class RoomTypeDescriptionChanged:IEvent
    {
        public readonly Guid TypeId;
        public readonly string Description;

        public RoomTypeDescriptionChanged(
            Guid typeId,
            string description)
        {
            TypeId = typeId;
            Description = description;
        }
    }
}
