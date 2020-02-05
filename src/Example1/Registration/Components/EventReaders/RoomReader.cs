using System;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using Infrastructure;
using Registration.Blueprint.Events;
using Registration.Blueprint.ReadModels;

namespace Registration.Components.EventReaders
{
    public class RoomsReader:
        Reader<List<RoomSummary>>
    {
        public RoomsReader(
            Func<IEventStoreConnection> conn,
            Func<ResolvedEvent, object> deserializer) :
            base(conn, deserializer){}

        private void Apply(RoomAdded evt)
        {
            Model.Add(new RoomSummary(
                            evt.RoomId,
                            evt.RoomNumber,
                            evt.RoomLocation,
                            evt.RoomType));
        }
    }
}
