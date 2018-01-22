using System;
using EventToolkit;
using FluentAssertions;
using System.Transactions;
using Kekiri.TestRunner.xUnit;

namespace Specs
{
    public class TransactionScenarios : EventScenarios
    {
        bool notified;
        TransactionScope trx;

        [Scenario]
        public void Can_recieve_notifications_in_a_transaction()
        {
            Given(a_transaction_scope)
                .And(an_event_subscription)
                .And(an_event_is_published);
            When(the_transaction_is_committed);
            Then(it_notifies_the_subscribers);
        }

        [Scenario]
        public void Subscribers_not_notified_when_transaction_is_cancelled()
        {
            Given(a_transaction_scope)
                .And(an_event_subscription)
                .And(an_event_is_published);
            When(the_transaction_is_cancelled);
            Then(it_does_not_notify_the_subscribers);
        }

        [Scenario]
        public void Exception_in_subscriber_does_not_cancel_transaction()
        {
            Given(a_transaction_scope)
                .And(an_unsafe_subscription)
                .And(an_event_subscription)
                .And(an_event_is_published);
            When(the_transaction_is_committed);
            Then(the_next_subscriber_is_notified)
                .And(no_exception_is_raised_outside_the_transaction);
        }
        
        void a_transaction_scope()
        {
            trx = new TransactionScope();
        }

        void an_event_subscription()
        {
            EventBus.Subscribe<Event>(_ => notified = true);
        }

        void an_unsafe_subscription()
        {
            EventBus.Subscribe<Event>(_ => { throw new StackOverflowException(); });
        }

        void an_event_is_published()
        {
            EventBus.Publish(new Event());
        }

        void the_transaction_is_committed()
        {
            trx.Complete();
            trx.Dispose();
        }

        void the_transaction_is_cancelled()
        {
            trx.Dispose();
        }

        void it_notifies_the_subscribers()
        {
            notified.Should().BeTrue();
        }

        void it_does_not_notify_the_subscribers()
        {
            notified.Should().BeFalse();
        }
    
        void no_exception_is_raised_outside_the_transaction()
        {
        }

        void the_next_subscriber_is_notified() => it_notifies_the_subscribers();
    }
}
