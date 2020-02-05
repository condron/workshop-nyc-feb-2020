using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Infrastructure;
using Newtonsoft.Json;
using Registration.Application;
using Registration.Blueprint.Commands;
using Registration.Blueprint.Events;
using Registration.Components.CommandHandlers;
using Registration.Components.EventReaders;

namespace Registration
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
            var conn = EventStoreConnection.Create(settings, IPEndPoint.Parse("127.0.0.1:1113"));
            conn.ConnectAsync().Wait();

            var eventNamespace = "Registration.Blueprint.Events";
            var eventAssembly = "Registration";

            var repo = new SimpleRepo(conn, eventNamespace, eventAssembly);

            var roomRm = new RoomsReader(() => conn, repo.Deserialize);

            var mainBus = new SimpleBus();

            var roomSvc = new RoomSvc(repo);
            mainBus.Subscribe<AddRoom>(roomSvc);
            
            var view = new ConsoleView();
            var controller = new Controller(view, mainBus);

            roomRm.Subscribe( model => view.RoomSummaries = model);

            roomRm.Start();

            controller.StartCommandLoop();
            
            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }
    }
}
