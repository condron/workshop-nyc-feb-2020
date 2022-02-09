using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccountDomain.Infrastructure
{
    public class Repository : IRepository
    {
        private Dictionary<string, List<IEvent>> _backingStore = new Dictionary<string, List<IEvent>>();
        public TAggregate GetbyId<TAggregate>(Guid id) where TAggregate : IEventDrivenStateMachine
        {
            TAggregate aggregate = (TAggregate)FormatterServices.GetUninitializedObject(typeof(TAggregate));
            var stream = $"{aggregate.Name}-{id:N}";
            if (!_backingStore.ContainsKey(stream))
            {
                throw new ArgumentOutOfRangeException("Aggregate not found!!!");
            }
            var events = _backingStore[stream];
            foreach (var @event in events) {
                aggregate.Apply(@event);
            }
            return aggregate;
        }
        //todo:add optimistic concurrency
        public bool Save(IEventSource source)
        {
            var eventData = source.TakeEvents();           
            var stream = $"{source.Name}-{source.Id:N}";
            if (!_backingStore.ContainsKey(stream)) { 
                _backingStore.Add(stream, new List<IEvent>());
            }
            _backingStore[stream].AddRange(eventData);
            return true;
        }
    }
}
