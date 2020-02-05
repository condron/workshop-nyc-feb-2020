using System;
using Infrastructure.Interfaces;

namespace Administration.EventModel.Commands
{
    public class AddRoomType:ICommand
    {
        public readonly Guid TypeId;
        public readonly string Description;

        public AddRoomType(
            Guid typeId,
            string description)
        {
            TypeId = typeId;
            Description = description;
        }
    }
}
