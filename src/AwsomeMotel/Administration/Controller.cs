using System;
using System.Linq;
using Administration.EventModel.Commands;
using Infrastructure.Interfaces;

namespace Administration
{
    public class Controller
    {
        private ConsoleView _view;
        private readonly IBus _mainBus;

        public Controller(ConsoleView view, IBus mainBus)
        {
            _view = view;
            _mainBus = mainBus;
        }
        public void StartCommandLoop()
        {
            do //Command loop
            {
                var cmd = Console.ReadLine();
                //Single token commands
                if (cmd.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Disconnecting EventStore");
                    break;
                }
                if (cmd.Equals("list", StringComparison.OrdinalIgnoreCase))
                {
	                _view.ListRooms();
	                continue;
                }
                if (cmd.Equals("list-types", StringComparison.OrdinalIgnoreCase))
                {
	                _view.ListRooms();
	                continue;
                }
                //3 token commands
                var tokens = cmd.Split(' ');
                if (tokens.Length < 2)
                {
                    _view.ErrorMsg = "Unknown command or Invalid number of parameters.";
                    continue;
                }
                switch (tokens[0].ToUpperInvariant())
                {
	                case "ADD-TYPE":
		                var addRoom = new AddRoomType(
			                Guid.NewGuid(),
			                tokens[1],
			                string.Join(" ", tokens.Skip(2)));

		                _mainBus.Publish(addRoom);
                       
		                break;
	                case "ADD-ROOM":
		                var typeId = _view.RoomSummaries.First(type => type.Description == tokens[1]).Id;
		                var room = new AddRoom(
			                Guid.NewGuid(),
			                typeId,
			                tokens[2]);

		                _mainBus.Publish(room);
                       
		                break;
	                default:
		                _view.ErrorMsg = "Unknown Command";
		                break;
                }
              

            } while (true);
        }
    }
}