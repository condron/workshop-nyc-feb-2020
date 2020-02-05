using System.Collections.Generic;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Infrastructure;
using Registration.Blueprint.Commands;
using Registration.Blueprint.ReadModels;
using Registration.Components.CommandHandlers;
using Registration.Components.EventReaders;

namespace Registration.Application
{
    public static class Bootstrap
    {
        public static IBus MainBus;
        public static IReadModel<List<RoomSummary>> RoomReadModel;
        public static void AsService()
        {
            //Bootstrap
            var settings = ConnectionSettings.Create()
                .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                .KeepReconnecting()
                .KeepRetrying()
                //.UseConsoleLogger()
                .Build();
            var conn = EventStoreConnection.Create(settings, IPEndPoint.Parse("127.0.0.1:1113"));
            conn.ConnectAsync().Wait();

            var eventNamespace = "Registration.Blueprint.Events";
            var eventAssembly = "Registration";

            var repo = new SimpleRepo(conn, eventNamespace, eventAssembly);

            var rm = new RoomsReader(() => conn, repo.Deserialize);

            MainBus = new SimpleBus();

            var roomSvc = new RoomSvc(repo);
            MainBus.Subscribe<AddRoom>(roomSvc);
            
            rm.Start();
            RoomReadModel = rm;

        }
        public static void ConfigureApp(RegistrationApp app, string eventNamespace, string eventAssembly)
        {
            var settings = ConnectionSettings.Create()
                .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                .KeepReconnecting()
                .KeepRetrying()
                //.UseConsoleLogger()
                .Build();
            var conn = EventStoreConnection.Create(settings, IPEndPoint.Parse("127.0.0.1:1113"));
            conn.ConnectAsync().Wait();


            var repo = new SimpleRepo(conn, eventNamespace, eventAssembly);

            var userRm = new RegisteredUsers(() => conn, repo.Deserialize);

            var mainBus = new SimpleBus();

            var userSvc = new UserSvc(repo);
            mainBus.Subscribe<RegisterUser>(userSvc);
            mainBus.Subscribe<ChangeName>(userSvc);

            //application wire up
            app.CommandPublisher = mainBus;
            userRm.Subscribe(app.DisplayUsers);
            //start 
            userRm.Start();


        }
    }
}
