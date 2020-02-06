using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Administration
{
    public static class Bootstrap
    {
        public static IBus MainBus;
       // public static IReadModel<List<RoomSummary>> RoomReadModel;
        public static void AsService()
        {
            //Bootstrap
            var settings = ConnectionSettings.Create()
                .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                .KeepReconnecting()
                .KeepRetrying()
                //.UseConsoleLogger()
                .Build();
            var conn = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113));
            conn.ConnectAsync().Wait();

            var eventNamespace = "Registration.Blueprint.Events";
            var eventAssembly = "Registration";

            var repo = new SimpleRepo(conn, eventNamespace, eventAssembly);

            //TODO: Setup Readers
            //var rm = new RoomsReader(() => conn, repo.Deserialize);

            MainBus = new SimpleBus();

            //TODO: Setup Services & subscribe to commands
            //var roomSvc = new RoomSvc(repo);
            //MainBus.Subscribe<AddRoom>(roomSvc);
            
            //rm.Start();
            //RoomReadModel = rm;

        }
       
    }
}
