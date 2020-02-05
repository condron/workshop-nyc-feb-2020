using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Administration.Blueprint.Commands;
using Administration.Components.CommandHandlers;
using Administration.Components.EventWriters;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Infrastructure;
using Infrastructure.Interfaces;
using Xunit;

namespace Administration.Tests
{
    public class admin_svc_tests
    {
        [Fact]
        public void can_create_room_type()
        {
            var eventNamespace = "Administration.Blueprint.Events";
            var eventAssembly = "Administration";
            var settings = ConnectionSettings.Create()
                .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                .KeepReconnecting()
                .KeepRetrying()
                //.UseConsoleLogger()
                .Build();
            var conn = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Parse("127.0.0.1"),1113));
            conn.ConnectAsync().Wait();


            var repo = new SimpleRepo(conn, eventNamespace, eventAssembly);
            var roomSvc = new AdminSvc(repo);
            var roomTypeId = Guid.NewGuid();
            roomSvc.Handle(new AddRoomType(roomTypeId, "King"));

            var room = repo.Load<RoomType>(roomTypeId);
            Assert.Equal(roomTypeId, ((IEventSource) room).Id);
        }
    }
}
