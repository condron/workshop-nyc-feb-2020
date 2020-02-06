using System;

namespace Administration.EventModel.ReadModels
{
    public class RoomTypeItem
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Description;

        public RoomTypeItem(Guid id, string name,string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public override string ToString()
        {
            return $"Room Type: {Description}";
        }
    }
}
