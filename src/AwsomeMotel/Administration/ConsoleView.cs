using System;
using System.Collections.Generic;
using Administration.EventModel.ReadModels;

namespace Administration
{
    public class ConsoleView
    {
        //hack reactive bindings 
        private List<RoomTypeItem> _rooms;

        public List<RoomTypeItem> RoomSummaries
        {
            get => _rooms;
            set
            {
                _rooms = value;
                ListRooms();
            }
        }

        private string _errorMsg;

        public string ErrorMsg
        {
            get => _errorMsg;
            set
            {
                _errorMsg = value;
                Error();
            }
        }

        public void Redraw()
        {
            Console.Clear();
            Console.WriteLine("Available Commands:");
            Console.WriteLine("\t Add-Type [name] [description]");
            Console.WriteLine("\t Add-Room [RoomType] [Room Number]");
            Console.WriteLine("\t list");
            Console.WriteLine("\t exit");
            Console.WriteLine("Command:");
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
                Console.WriteLine(room);
            }
        }
    }
}