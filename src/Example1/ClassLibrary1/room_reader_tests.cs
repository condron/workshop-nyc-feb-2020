using System;
using System.Collections.Generic;
using System.Text;
using Registration.Blueprint.Events;
using Registration.Components.EventReaders;
using Xunit;

namespace Registration.Tests
{
    public class room_reader_tests
    {
        [Fact]
        public void can_create_room_summary()
        {
            var room1 = new RoomAdded(Guid.NewGuid(), "11a", "1st Floor", "Queen");
            var room2 = new RoomAdded(Guid.NewGuid(), "11b", "1st Floor", "Queen");
            var room3 = new RoomAdded(Guid.NewGuid(), "11c", "1st Floor", "Queen");

            var reader = new RoomsReader(() => null, null);

            reader.Apply(room1);
            reader.Apply(room2);
            reader.Apply(room3);

            var roomSummaries = reader.Current;

            Assert.Collection(
                roomSummaries,
                s => {
                    Assert.Equal(room1.RoomId, s.RoomId);
                    Assert.Equal(
                        $"Room: {room1.RoomNumber} Floor: {room1.RoomLocation} Type: {room1.RoomType} Id: {room1.RoomId}", 
                        s.Summary);
                },
                s => {
                    Assert.Equal(room2.RoomId, s.RoomId);
                    Assert.Equal(
                        $"Room: {room2.RoomNumber} Floor: {room2.RoomLocation} Type: {room2.RoomType} Id: {room2.RoomId}", 
                        s.Summary);
                },
                s => {
                    Assert.Equal(room3.RoomId, s.RoomId);
                    Assert.Equal(
                        $"Room: {room3.RoomNumber} Floor: {room3.RoomLocation} Type: {room3.RoomType} Id: {room3.RoomId}", 
                        s.Summary);
                });

        }
    }
}
