namespace Infrastructure.Interfaces
{
    public interface IPublish{
        void Publish(IMessage message);
    }
}