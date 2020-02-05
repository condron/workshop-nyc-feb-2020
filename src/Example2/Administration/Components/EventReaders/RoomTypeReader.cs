using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Administration.Blueprint.Events;
using Administration.Blueprint.ReadModels;
using EventStore.ClientAPI;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Administration.Components.EventReaders
{
    public class RoomTypeReader:
        Reader<List<RoomTypeItem>>
    {
        public RoomTypeReader(Func<IEventStoreConnection> conn, Func<ResolvedEvent, object> deserializer)
            :base(conn,deserializer)
        {
        }

        private void Apply(RoomTypeAdded evt)
        {
            Model.Add(new RoomTypeItem(evt.TypeId,evt.Description));
        }

        private void Apply(RoomTypeDescriptionChanged evt)
        {
            var rt = Model.FirstOrDefault(i => i.Id == evt.TypeId);
            if (rt != null) {
                Model.Remove(rt);
                Model.Add(new RoomTypeItem(evt.TypeId, evt.Description));
            }
        }
    }
}
