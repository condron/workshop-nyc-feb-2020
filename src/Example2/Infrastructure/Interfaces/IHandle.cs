namespace Infrastructure.Interfaces{
    public interface IHandle<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}