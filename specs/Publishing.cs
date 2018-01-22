using EventToolkit;
using Kekiri;
using FluentAssertions;
using Kekiri.TestRunner.xUnit;

namespace Specs
{
    public class PublishingScenarios : EventScenarios
    {
        string notified = string.Empty;
        IEventSubscription lastSubscription;

        SimpleSubscriber subscriber;

        [Scenario]
        public void Event_can_be_published_to_a_single_subscriber()
        {
            Given(a_single_subscriber);
            When(publishing_an_event);
            Then(it_notifies_the_subscriber);
        }

        [Scenario]
        public void Event_can_be_published_to_multiple_subscribers()
        {
            Given(multiple_subscribers);
            When(publishing_an_event);
            Then(it_notifies_all_subscribers);
        }

        [Scenario]
        public void Subscribers_can_subscribe_to_events_of_event_hierarchy()
        {
            Given(subscription_to_base_and_derived_event);
            When(publishing_a_derived_event);
            Then(it_notifies_the_subscribers_of_the_base_type)
                .And(it_notifies_the_subscribers_of_the_derived_type);
        }

        [Scenario]
        public void Subscribers_of_base_event_are_not_notified_of_derived_events()
        {
            Given(subscription_to_base_and_derived_event);
            When(publishing_a_base_event);
            Then(it_notifies_the_subscribers_of_the_base_type)
                .And(it_does_not_notify_the_subscribers_of_the_derived_type);
        }

        [Scenario]
        public void Disposed_delegate_is_not_notified()
        {
            Given(a_single_subscriber)
                .And(that_subscription_is_disposed);
            When(publishing_an_event);
            Then(it_does_not_notify_the_subscriber);
        }

        [Scenario]
        public void Disposed_subscription_disposes_subscriber()
        {
            Given(a_subscriber)
                .And(that_subscription_is_disposed);
            When(publishing_an_event);
            Then(the_subscriber_is_disposed);
        }

        void a_single_subscriber()
        {
            lastSubscription = EventBus.Subscribe<Event>(_ => notified = "yes");
        }

        void a_subscriber()
        {
            subscriber = new SimpleSubscriber();
            lastSubscription = EventBus.Subscribe<Event>(subscriber);
        }

        void multiple_subscribers()
        {
            EventBus.Subscribe<Event>(_ => notified += "a");
            EventBus.Subscribe<Event>(_ => notified += "b");
            EventBus.Subscribe<Event>(_ => notified += "c");
        }

        void subscription_to_base_and_derived_event()
        {
            EventBus.Subscribe<BaseEvent>(_ => notified += "base");
            EventBus.Subscribe<DerivedEvent>(_ => notified += "derived");
        }

        void that_subscription_is_disposed()
        {
            lastSubscription.Dispose();
        }

        void publishing_an_event()
        {
            EventBus.Publish(new Event());
        }

        void publishing_a_base_event()
        {
            EventBus.Publish(new BaseEvent());
        }

        void publishing_a_derived_event()
        {
            EventBus.Publish(new DerivedEvent());
        }

        void it_notifies_the_subscriber()
        {
            notified.Should().Be("yes");
        }

        void it_notifies_all_subscribers()
        {
            notified.Should().Be("abc");
        }

        void it_does_not_notify_the_subscriber()
        {
            notified.Should().BeNullOrEmpty();
        }

        void it_notifies_the_subscribers_of_the_base_type()
        {
            notified.Should().Contain("base");
        }

        void it_notifies_the_subscribers_of_the_derived_type()
        {
            notified.Should().Contain("derived");
        }
        
        void it_does_not_notify_the_subscribers_of_the_derived_type()
        {
            notified.Should().NotContain("derived");
        }

        void the_subscriber_is_disposed()
        {
            subscriber.disposed.Should().BeTrue();
        }
    }
}
