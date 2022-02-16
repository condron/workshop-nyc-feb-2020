using ReactiveDomain;
using ReactiveDomain.Foundation;
using ReactiveDomain.Messaging;
using ReactiveDomain.Messaging.Bus;
using System;
using System.Collections.Generic;

namespace AccountReadModels
{

    public class ContentTypesRm : ReadModelBase,
          IHandle<ContentTypeCreated>
    {
        private readonly Func<IListener> getListener;
        public List<ContentTypeDisplayDTO> ContentTypes { get; private set; } = new List<ContentTypeDisplayDTO>();
        public ContentTypeRm GetContentTypeRm(ContentTypeDisplayDTO contentType)
        {
            //todo: cache these if needed as any created DTO is read only
            return new ContentTypeRm(contentType.Id, getListener);
        }
        public ContentTypesRm(Func<IListener> getListener) : base(nameof(ContentTypesRm), getListener)
        {
            this.getListener = getListener;

        }
        public void Handle(ContentTypeCreated @event)
        {
            ContentTypes.Add(new ContentTypeDisplayDTO { Id = @event.Id, Name = @event.Name });
        }
    }

    //simple DTO we might use to populate a pick list
    public class ContentTypeDisplayDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
    }
    public class ContentTypeRm : ReadModelBase,
        IHandle<ContentTypeCreated>,
        IHandle<SectionAdded>,
        IHandle<WidgetAdded>
    {

        private Dictionary<Guid, SectionRm> _sections = new Dictionary<Guid, SectionRm>();

        public ContentTypeDTO ContentType { get; private set; } = null;
        public ContentTypeRm(Guid contentTypeId, Func<IListener> getListener) : base(nameof(ContentTypeRm), getListener)
        {
            EventStream.Subscribe<ContentTypeCreated>(this);
            EventStream.Subscribe<SectionAdded>(this);
            EventStream.Subscribe<WidgetAdded>(this);

            Start<ContentTypeAgg>(contentTypeId);

        }

        public void Handle(ContentTypeCreated @event)
        {
            if (ContentType != null) { return; }//this shouldn't happen 
            ContentType = new ContentTypeDTO(/*map parms here from event*/);
        }

        public void Handle(SectionAdded @event)
        {
            //create a new section readmodel to handle events for that section
            var sectionRm = new SectionRm();
            sectionRm.Handle(@event);
            _sections.Add(@event.SectionId, sectionRm);

            //delegate the event handle to the created readmodel
            ContentType.AddSection(sectionRm.Section);
        }

        public void Handle(WidgetAdded @event)
        {
            //find an existing readmodel to handle this event 
            if (!_sections.TryGetValue(@event.SectionId, out var section)) { return; }
            section.Handle(@event);
        }


    }
}

//n.b.: only do this if there is enough complexity to warrant the ecapsulation. else just handle all events directly in the readmodel and update the DTO directly
public class SectionRm :
     IHandle<SectionAdded>,
     IHandle<WidgetAdded>
{
    public SectionDTO Section { get; private set; }
    public SectionRm()
    {
        Section = new SectionDTO(/*add parms*/);
    }

    public void Handle(SectionAdded message)
    {
        throw new NotImplementedException();
    }

    public void Handle(WidgetAdded message)
    {
        throw new NotImplementedException();
    }

}
public class ContentTypeDTO
{
    public ContentTypeDTO()
    {

    }
    public void AddSection(SectionDTO section)
    {
    }
}
public class SectionDTO
{
    public SectionDTO()
    {

    }
}
public class ContentTypeAgg : AggregateRoot
{
}
public class ContentTypeCreated : Event {
    public Guid Id { get; set; }
    public String Name { get; set; }

}
public class SectionAdded : Event
{
    public Guid SectionId { get; }
}
public class WidgetAdded : Event
{
    public Guid SectionId { get; }
}
}
