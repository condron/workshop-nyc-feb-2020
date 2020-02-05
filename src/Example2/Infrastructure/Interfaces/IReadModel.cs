using System;
using EventStore.ClientAPI;

namespace Infrastructure.Interfaces {
    public interface IReadModel<T>
    {
        T Current { get; }
        T At(Position checkpoint);
        SnapShot<T> Snapshot { get; }
        void Subscribe(Action<T> target);
    }
}