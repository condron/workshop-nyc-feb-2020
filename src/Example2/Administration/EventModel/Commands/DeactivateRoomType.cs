using System;
using Infrastructure.Interfaces;

namespace Administration.EventModel.Commands
{
    public class DeactivateRoomType:ICommand
    {
        public readonly Guid TypeId;

        public DeactivateRoomType(
            Guid typeId)
        {
            TypeId = typeId;
        }
    }
}
