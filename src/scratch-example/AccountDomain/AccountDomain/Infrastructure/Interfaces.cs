using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDomain.Infrastructure
{
    public interface IMessage { }
    public interface ICommand { }
    public interface IEvent { }
    public interface IHandleCommand<ICommand> { bool Handle(ICommand cmd); };
    public interface IHandle<IEvent> { bool Handle(IEvent @event); };
    public interface IEventSource {
        Guid Id { get; }
        string Name { get; }
        List<IEvent> TakeEvents();
    }
    public interface IEventDrivenStateMachine: IEventSource
    {
        void Apply(IEvent @event);
    }
    public interface IRepository {
        TAggregate GetbyId<TAggregate>(Guid id) where TAggregate : IEventDrivenStateMachine;
        bool Save(IEventSource source);
    }
}
