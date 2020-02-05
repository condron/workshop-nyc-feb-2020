using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Infrastructure;
using Registration.Blueprint.Commands;
using Registration.Components.CommandHandlers;
using Registration.Components.EventWriters;
using Xunit;

namespace Registration.Tests
{
    public class room_integration_tests
    {
        [Fact]
        public void addRoom_command_will_save()
        {
            var eventNamespace = "Registration.Blueprint.Events";
            var eventAssembly = "Registration";
            var settings = ConnectionSettings.Create()
                .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                .KeepReconnecting()
                .KeepRetrying()
                //.UseConsoleLogger()
                .Build();
            var conn = EventStoreConnection.Create(settings, IPEndPoint.Parse("127.0.0.1:1113"));
            conn.ConnectAsync().Wait();


            var repo = new SimpleRepo(conn, eventNamespace, eventAssembly);
            var roomSvc = new RoomSvc(repo);
            var roomId = Guid.NewGuid();
            roomSvc.Handle(new AddRoom(roomId, "12B", "2nd Floor", "King"));

            var room = repo.Load<Room>(roomId);
            Assert.Equal(roomId, ((IEventSource)room).Id);
        }
    }
}
