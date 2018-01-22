using System;
using System.Threading;

namespace EventToolkit
{
    public class EventBus
    {
        static AsyncLocal<ScopedEventBus> bus = new AsyncLocal<ScopedEventBus>();
        static ScopedEventBus Bus => bus.Value ?? (bus.Value = EventCore.CreateScope());

        public static IEventBus Current => Bus;

        public static IEventSubscription Subscribe<TEvent>(Action<TEvent> handler)
          where TEvent : IEvent
        {
            return Bus.Subscribe(handler);
        }

        public static IEventSubscription Subscribe<TEvent>(IEventSubscriber subscriber)
          where TEvent : IEvent
        {
            return Bus.Subscribe<TEvent>(subscriber);
        }

        public static IEventSubscription Subscribe(Type eventType, IEventSubscriber subscriber)
        {
            return Bus.Subscribe(eventType, subscriber);
        }

        public static void Publish<TEvent>(TEvent eventMessage)
          where TEvent : IEvent
        {
            Bus.Publish(eventMessage);
        }

        public static void Clear()
        {
            if (bus.Value == null) return;
            var value = bus.Value;
            bus.Value = null;
            value.Clear();
        }
    }
}
