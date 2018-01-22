using EventToolkit;
using FluentAssertions;
using Kekiri.TestRunner.xUnit;

namespace Specs
{
    public class ScopeScenarios : EventScenarios
    {
        protected SimpleSubscriber localSubscriber = new SimpleSubscriber();
        protected SimpleSubscriber globalSubscriber = new SimpleSubscriber();

        [Scenario]
        public void events_are_published_to_local_and_global_subscribers()
        {
            Given(a_global_subscription)
                .And(a_local_subscription);
            When(publishing_an_event);
            Then(it_sends_an_event_to_handlers_at_the_local_scope)
                .And(it_sends_an_event_to_handlers_at_the_application_scope);
        }
        
        void a_global_subscription()
        {
            EventMonitor.Monitor<Event>(globalSubscriber);
        }

        void a_local_subscription()
        {
            EventBus.Subscribe<Event>(localSubscriber);
        }

        void publishing_an_event()
        {
            EventBus.Publish(new Event());
        }

        void it_sends_an_event_to_handlers_at_the_local_scope()
        {
            localSubscriber.handled.Should().BeTrue();
        }

        void it_sends_an_event_to_handlers_at_the_application_scope()
        {
            globalSubscriber.handled.Should().BeTrue();
        }
    }
}

