using ReactiveDomain.EventStore;
using ReactiveDomain.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ES = EventStore.ClientAPI;

namespace Application
{
    public static class Bootstrap
    {
        
        public static IConfiguredConnection ESConnection;
       

        public static void Startup() { 
            ESConnection = BuildConnection();
        }

        private static IConfiguredConnection BuildConnection()
        {
            string esUser = "admin";
            string esPwd = "changeit";
            string esIpAddress = "localhost";
            int esPort = 1113;

            var tcpEndpoint = new IPEndPoint(IPAddress.Parse(esIpAddress), esPort);

            var settings = ES.ConnectionSettings.Create()
                .SetDefaultUserCredentials(new ES.SystemData.UserCredentials(esUser, esPwd))
                .KeepReconnecting()
                .KeepRetrying()
                .UseConsoleLogger()
                .DisableTls()
                .DisableServerCertificateValidation()
                .WithConnectionTimeoutOf(TimeSpan.FromSeconds(15))
                .Build();

            var conn = ES.EventStoreConnection.Create(settings, tcpEndpoint, "ES Connection");

            conn.ConnectAsync().Wait();
            return new ConfiguredConnection(
                new EventStoreConnectionWrapper(conn),
                new PrefixedCamelCaseStreamNameBuilder(),
                new JsonMessageSerializer());

        }
    }
}
