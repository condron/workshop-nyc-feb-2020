using System;
using System.Net;
using Administration.Components.CommandHandlers;
using Administration.Components.EventReaders;
using Administration.EventModel.Commands;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Infrastructure;

namespace Administration
{
    class Program
    {
        static void Main(string[] args)
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

            var eventNamespace = "Administration.Blueprint.Events";
            var eventAssembly = "Administration";

            var repo = new SimpleRepo(conn, eventNamespace, eventAssembly);

            var roomTypeRm = new RoomTypeReader(() => conn, repo.Deserialize);

            var mainBus = new SimpleBus();

            var adminSvc = new AdminSvc(repo);
            mainBus.Subscribe<AddRoomType>(adminSvc);

            var view = new ConsoleView();
            var controller = new Controller(view, mainBus);

            roomTypeRm.Subscribe(model => view.RoomSummaries = model);

            view.Redraw();
            roomTypeRm.Start();
            controller.StartCommandLoop();

            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }
    }
}