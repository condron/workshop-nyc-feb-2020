using System;
using System.Runtime.Serialization;
using Infrastructure;
using Registration.Blueprint.Events;
using Registration.Components.EventWriters;
using Xunit;
// ReSharper disable InconsistentNaming

namespace Registration.Tests
{
    public class room_tests
    {
        [Fact]
        public void can_add_room()
        {
            //given

            //when
            var roomId = Guid.NewGuid();
            var roomNumber = "11A";
            var roomLocation = "Basement";
            var roomType = "Economy";
            var user = new Room(roomId, roomNumber, roomLocation, roomType);

            //then
            var events = ((IEventSource)user).TakeEvents();
            Assert.Collection(
                events,
                (evt) => {
                    var room = evt as RoomAdded;
                    Assert.NotNull(room);
                    Assert.Equal(roomId, room.RoomId);
                    Assert.Equal(roomNumber, room.RoomNumber);
                    Assert.Equal(roomLocation, room.RoomLocation);
                    Assert.Equal(roomType, room.RoomType);
                });
        }

        [Fact]
        public void cannot_add_room_with_empty_id()
        {
            var roomId = Guid.Empty;
            var roomNumber = "11A";
            var roomLocation = "Basement";
            var roomType = "Economy";
            Assert.Throws<ArgumentNullException>(() => new Room(roomId, roomNumber, roomLocation, roomType));
        }
        [Fact]
        public void cannot_add_room_with_empty_number()
        {
            var roomId = Guid.NewGuid();
            var roomNumber = "";
            var roomLocation = "Basement";
            var roomType = "Economy";
            Assert.Throws<ArgumentNullException>(() => new Room(roomId, roomNumber, roomLocation, roomType));
        }
        [Fact]
        public void cannot_add_room_with_empty_location()
        {
            var roomId = Guid.NewGuid();
            var roomNumber = "11A";
            var roomLocation = "";
            var roomType = "Economy";
            Assert.Throws<ArgumentNullException>(() => new Room(roomId, roomNumber, roomLocation, roomType));
        }
        [Fact]
        public void cannot_add_room_with_empty_type()
        {
            var roomId = Guid.NewGuid();
            var roomNumber = "11A";
            var roomLocation = "Basement";
            var roomType = "";
            Assert.Throws<ArgumentNullException>(() => new Room(roomId, roomNumber, roomLocation, roomType));
        }
    }
}
