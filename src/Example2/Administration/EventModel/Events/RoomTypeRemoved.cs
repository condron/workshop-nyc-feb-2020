using System;
using Infrastructure.Interfaces;

namespace Administration.EventModel.Events
{
    public class RoomTypeDeactivated:IEvent
    {
        public readonly Guid TypeId;

        public RoomTypeDeactivated(
            Guid typeId)
        {
            TypeId = typeId;
        }
    }
}
