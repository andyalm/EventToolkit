using System;
using EventToolkit;
using Kekiri;
using FluentAssertions;
using Kekiri.TestRunner.xUnit;

namespace Specs
{
    public class SubscribingScenarios : EventScenarios
    {
        IEventSubscription subscription;
        
        [Scenario]
        public void Can_subscribe_to_an_event_with_a_delegate()
        {
            When(subscribing_with_a_delegate);
            Then(it_creates_a_subscription);
        }

        [Scenario]
        public void Can_subscribe_to_an_event_with_a_subscriber()
        {
            When(subscribing_with_a_subscriber);
            Then(it_creates_a_subscription);
        }

        [Scenario]
        public void Can_subscribe_with_the_current_event_bus()
        {
            When(subscribing_with_the_current_event_bus);
            Then(it_creates_a_subscription);
        }

        [Scenario]
        public void Cannot_subscribe_with_a_null_delegate()
        {
            When(attempting_to_subscribe_with_a_null_delegate).Throws();
            Then(argumentnullexception_is_thrown);
        }

        [Scenario]
        public void Cannot_subscribe_with_a_null_subscriber()
        {
            When(attempting_to_subscribe_with_a_null_subscriber).Throws();
            Then(argumentnullexception_is_thrown);
        }

        
        
        void subscribing_with_a_delegate()
        {
            subscription = EventBus.Subscribe<Event>(_ => { });
        }

        void subscribing_with_a_subscriber()
        {
            subscription = EventBus.Subscribe<Event>(new SimpleSubscriber());
        }

        void subscribing_with_the_current_event_bus()
        {
            subscription = EventBus.Current.Subscribe<Event>(_ => { });
        }

        void attempting_to_subscribe_with_a_null_delegate()
        {
            EventBus.Subscribe((Action<Event>)null);
        }

        void attempting_to_subscribe_with_a_null_subscriber()
        {
            EventBus.Subscribe<Event>((IEventSubscriber)null);
        }

        void it_creates_a_subscription()
        {
            subscription.Should().NotBeNull();
        }

        void argumentnullexception_is_thrown()
        {
            Catch<ArgumentNullException>().Should().NotBeNull();
        }
    }
}
