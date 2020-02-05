using System;
using System.Collections.Generic;
using System.Text;
using Administration.Components.EventReaders;
using Administration.EventModel.Events;
using Administration.EventModel.ReadModels;
using Infrastructure;
using Xunit;

namespace Administration.Tests
{
    public class room_types_view_tests
    {
        [Fact]
        public void can_display_room_types()
        {
            //given 
            var rt1 = new RoomTypeAdded(Guid.NewGuid(), "1King");
            var rt2 = new RoomTypeAdded(Guid.NewGuid(), "2Queen");
            var rt3 = new RoomTypeAdded(Guid.NewGuid(), "3Econ");
            //when
            var rm = new RoomTypeReader(() => null, new SimpleRepo(null, "Administration.Blueprint.Events", "Administration").Deserialize);
            rm.Apply(rt1);
            rm.Apply(rt2);
            rm.Apply(rt3);
            //Then
            Assert.Collection(rm.Current,
                i => {
                    var item = i as RoomTypeItem;
                    Assert.NotNull(item);
                    Assert.Equal(rt1.TypeId, item.Id);
                    Assert.Equal(rt1.Description, item.Description, StringComparer.Ordinal);
                }, i => {
                    var item = i as RoomTypeItem;
                    Assert.NotNull(item);
                    Assert.Equal(rt2.TypeId, item.Id);
                    Assert.Equal(rt2.Description, item.Description, StringComparer.Ordinal);
                }, i => {
                    var item = i as RoomTypeItem;
                    Assert.NotNull(item);
                    Assert.Equal(rt3.TypeId, item.Id);
                    Assert.Equal(rt3.Description, item.Description, StringComparer.Ordinal);
                });
        }
        [Fact]
        public void can_display_updated_room_types()
        {
            //given 
            var rt1 = new RoomTypeAdded(Guid.NewGuid(), "1King");
            var rt2 = new RoomTypeAdded(Guid.NewGuid(), "2Queen");
            var rt3 = new RoomTypeAdded(Guid.NewGuid(), "3Econ");
            var update = new RoomTypeDescriptionChanged(rt1.TypeId, "1Super");
            //when
            var rm = new RoomTypeReader(() => null, new SimpleRepo(null, "Administration.Blueprint.Events", "Administration").Deserialize);
            rm.Apply(rt1);
            rm.Apply(rt2);
            rm.Apply(rt3);
            rm.Apply(update);
            //Then
            Assert.Collection(rm.Current,
                  i => {
                      var item = i as RoomTypeItem;
                      Assert.NotNull(item);
                      Assert.Equal(rt2.TypeId, item.Id);
                      Assert.Equal(rt2.Description, item.Description, StringComparer.Ordinal);
                  }, i => {
                      var item = i as RoomTypeItem;
                      Assert.NotNull(item);
                      Assert.Equal(rt3.TypeId, item.Id);
                      Assert.Equal(rt3.Description, item.Description, StringComparer.Ordinal);
                  },
                  i => {
                      var item = i as RoomTypeItem;
                      Assert.NotNull(item);
                      Assert.Equal(rt1.TypeId, item.Id);
                      Assert.Equal(update.Description, item.Description, StringComparer.Ordinal);
                  });
        }
    }
}
