using System;
using Infrastructure.Interfaces;

namespace Administration.EventModel.Commands
{
    public class AddRoomType:ICommand
    {
        public readonly Guid TypeId;
        public readonly string Name;
        public readonly string Description;

        public AddRoomType(
            Guid typeId,
			string name,
            string description)
        {
            TypeId = typeId;
            Name = name;
            Description = description;
        }
    }
}
