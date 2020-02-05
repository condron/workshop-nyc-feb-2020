using System;

namespace Infrastructure.Interfaces {
    public interface IRepository {
        void Save(IEventSource source);
        T Load<T>(Guid id) where T : class, IEventSource;
    }
}