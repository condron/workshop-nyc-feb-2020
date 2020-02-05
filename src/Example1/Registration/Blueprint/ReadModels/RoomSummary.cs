using System;

namespace Registration.Blueprint.ReadModels
{
    public class RoomSummary
    {
        public Guid RoomId;
        public string Summary;

        public RoomSummary(
            Guid roomId,
            string roomNumber,
            string roomLocation,
            string roomType
        )
        {
            RoomId = roomId;
            Summary = $"Room: {roomNumber} Floor: {roomLocation} Type: {roomType} Id: {roomId}";
        }
    }
}
