using System;
using System.Collections.Generic;
using System.Text;

namespace Administration.Blueprint.ReadModels
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
