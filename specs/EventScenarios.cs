using System;
using EventToolkit;
using Kekiri.TestRunner.xUnit;

namespace Specs
{
    public class EventScenarios : Scenarios, IDisposable
    {
        public virtual void Dispose()
        {
            EventBus.Clear();
        }
    }
}
