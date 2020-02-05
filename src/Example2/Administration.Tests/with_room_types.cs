using System;
using Administration.Blueprint.Events;
using Administration.Components.EventWriters;
using Infrastructure.Interfaces;
using Xunit;

namespace Administration.Tests
{
    public class with_room_types
    {
        [Fact]
        public void can_add_new_room_type()
        {
            //Given
            
            //When
            var id = Guid.NewGuid();
            var desc = "King";
            var type = new RoomType(id, desc);

            //then
            var events = ((IEventSource) type).TakeEvents();
            Assert.Collection(events,
                evt => {
                    var added = evt as RoomTypeAdded;
                    Assert.NotNull(added);
                    Assert.Equal(id,added.TypeId);
                    Assert.Equal(desc, added.Description,StringComparer.Ordinal);
                });
        }
    }
}
