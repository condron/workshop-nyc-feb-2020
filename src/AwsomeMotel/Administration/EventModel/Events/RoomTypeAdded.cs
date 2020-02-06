using System;
using Infrastructure.Interfaces;

namespace Administration.EventModel.Events
{
	public class RoomTypeAdded : IEvent
	{
		public readonly Guid TypeId;
		public readonly string Name;
		public readonly string Description;

		public RoomTypeAdded(
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
