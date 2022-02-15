using AccountDomain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountDomain
{
    public class MetaTagSharedRoot : IEventDrivenStateMachine
    {
        private List<IEvent> _pendingEvents = new List<IEvent>();
       
        public Guid Id => new Guid("7DCFDCF8-2E0A-48F7-A18B-B0599D1EE1AB");
        public string Name => GetType().Name;

        private Dictionary<string, Guid> _activeTags = new Dictionary<string, Guid>(StringComparer.Ordinal);
        private Dictionary<string, Guid> _retiredTags = new Dictionary<string, Guid>(StringComparer.Ordinal);

        //public behavoirs
        public MetaTagSharedRoot() {}

       public Guid AddTag(string tag) {
            if (string.IsNullOrWhiteSpace(tag)) { throw new ArgumentException("Empty tag is not allowed!"); }
            if (_activeTags.ContainsKey(tag)) { return _activeTags[tag]; } //if exists return idempotent success
            if (_retiredTags.ContainsKey(tag)) {  //if retired resore 
                var id = _retiredTags[tag];
                Raise(new MetatagMsgs.MetaTagRestored(id));
                return id;
            }           
            //else add new
            var newId = Guid.NewGuid();
            Raise(new MetatagMsgs.MetaTagAdded(newId, tag));
            return newId;
        }
        public void RenameTag(string oldTag, string newTag) {
            if (string.IsNullOrWhiteSpace(oldTag)) { throw new ArgumentException("Empty old tag is not allowed!"); }
            if (string.IsNullOrWhiteSpace(newTag)) { throw new ArgumentException("Empty new tag is not allowed!"); }

            //if not active throw
            if (!_activeTags.ContainsKey(oldTag)) { throw new ArgumentException("Tag not found!!!"); }
            //else rename
            Raise(new MetatagMsgs.MetaTagRenamed(_activeTags[oldTag], newTag));
        }
        public void RetireTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) { throw new ArgumentException("Empty tag is not allowed!"); }

            //if not active throw
            if (!_activeTags.ContainsKey(tag)) { throw new ArgumentException("Tag not found!!!"); }
            //else retire
            Raise(new MetatagMsgs.MetaTagRetired(_activeTags[tag]));
        }

        //apply methods
        private void Apply(MetatagMsgs.MetaTagAdded @event)
        {
            _activeTags.Add(@event.Tag, @event.TagId);
        }
        private void Apply(MetatagMsgs.MetaTagRenamed @event)
        {            
            _activeTags.Remove(GetActiveTag(@event.TagId));
            _activeTags.Add(@event.NewTag, @event.TagId);
        }
        private void Apply(MetatagMsgs.MetaTagRetired @event)
        {
            var oldTag = GetActiveTag(@event.TagId);
            _activeTags.Remove(oldTag);
            _retiredTags.Add(oldTag, @event.TagId);
        }
        private void Apply(MetatagMsgs.MetaTagRestored @event)
        {
            var oldTag = string.Empty;
            foreach (var entry in _retiredTags)
            {
                if (entry.Value == @event.TagId)
                {
                    oldTag =  entry.Key;
                    break;
                }
            }
            _retiredTags.Remove(oldTag);
            _activeTags.Add(oldTag, @event.TagId);
        }
        private string GetActiveTag(Guid id) {
            
            foreach (var entry in _activeTags)
            {
                if (entry.Value == id)
                {
                    return entry.Key;                    
                }
            }
            return string.Empty; //this should never happen
        }
        //todo: move into base class and use reflection or something
        private void Raise(IEvent @event)
        {
            _pendingEvents.Add(@event);
            Apply(@event);
        }
        public void Apply(IEvent @event)
        {
            if (@event.GetType() == typeof(MetatagMsgs.MetaTagAdded))
            {
                Apply((MetatagMsgs.MetaTagAdded)@event);
                return;
            }
            if (@event.GetType() == typeof(MetatagMsgs.MetaTagRenamed))
            {
                Apply((MetatagMsgs.MetaTagRenamed)@event);
                return;
            }
            if (@event.GetType() == typeof(MetatagMsgs.MetaTagRetired))
            {
                Apply((MetatagMsgs.MetaTagRetired)@event);
                return;
            }
            if (@event.GetType() == typeof(MetatagMsgs.MetaTagRestored))
            {
                Apply((MetatagMsgs.MetaTagRestored)@event);
                return;
            }
        }

        public List<IEvent> TakeEvents()
        {
            var @events = _pendingEvents;
            _pendingEvents = new List<IEvent>();
            return @events;
        }
    }
}
