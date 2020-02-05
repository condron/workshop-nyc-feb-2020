using System;

namespace Administration.EventModel.ReadModels
{
    public class RoomTypeItem
    {
        public readonly Guid Id;
        public readonly string Description;

        public RoomTypeItem(Guid id,string description)
        {
            Id = id;
            Description = description;
        }
        public override string ToString()
        {
            return $"Room Type: {Description}";
        }
    }
}
