using EventToolkit;
using FluentAssertions;
using Kekiri.TestRunner.xUnit;

namespace Specs
{
    public class MonitoringScenarios : EventScenarios
    {
        IEventSubscription subscription;
        
        [Scenario]
        public void Can_monitor_an_event_with_a_delegate()
        {
            When(calling_monitor_with_a_delegate);
            Then(it_creates_a_subscription);
        }

        [Scenario]
        public void Can_monitor_an_event_with_a_subscriber()
        {
            When(calling_monitor_with_a_subscriber);
            Then(it_creates_a_subscription);
        }

        void calling_monitor_with_a_delegate()
        {
            subscription = EventMonitor.Monitor<Event>(_ => { });
        }
        
        void calling_monitor_with_a_subscriber()
        {
            subscription = EventMonitor.Monitor<Event>(new SimpleSubscriber());
        }

        void it_creates_a_subscription()
        {
            subscription.Should().NotBeNull();
        }
    }
}
