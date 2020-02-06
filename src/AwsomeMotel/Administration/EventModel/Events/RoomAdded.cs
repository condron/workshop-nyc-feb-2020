using System;
using Infrastructure.Interfaces;

namespace Administration.EventModel.Events
{
	public class RoomAdded : IEvent
	{
		public readonly Guid RoomId;
		public readonly Guid TypeId;
		public readonly string RoomNumber;

		public RoomAdded(
			Guid roomId,
			Guid typeId,
			string roomNumber)
		{
			TypeId = typeId;
			RoomId = roomId;
			RoomNumber = roomNumber;
		}
	}
}
