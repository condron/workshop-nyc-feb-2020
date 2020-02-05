using System;
using System.Collections.Generic;
using Registration.Blueprint.ReadModels;

namespace Registration
{
    public class ConsoleView
    {
        //hack reactive bindings 
        private List<RoomSummary> _rooms;
        public List<RoomSummary> RoomSummaries
        { get => _rooms;
            set { _rooms = value; ListRooms(); } }

        private string _errorMsg;
        public string ErrorMsg { get => _errorMsg;
            set { _errorMsg = value; Error(); } }
        
        public void Redraw()
        {

            Console.Clear();
            Console.WriteLine("Available Commands:");
            Console.WriteLine("\t Add [number] [location] [type]");
            Console.WriteLine("\t list");
            Console.WriteLine("\t exit");
            Console.Write("Command:");
        }

        private void Error()
        {
            Console.WriteLine();
            Console.WriteLine("Error: " + _errorMsg);
            Console.WriteLine("Press enter to retry");
            Console.ReadLine();
            Redraw();
        }

        public void ListRooms()
        {
            Redraw();
            foreach (var room in _rooms)
            {
                Console.WriteLine(room.Summary);
            }
        }
    }
}