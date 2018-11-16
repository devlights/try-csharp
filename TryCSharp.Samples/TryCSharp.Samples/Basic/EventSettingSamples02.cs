using System;
using System.ComponentModel;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     手動でイベントを制御する方法に関してのサンプルです。(EventHandlerList)
    /// </summary>
    [Sample]
    public class EventSettingSamples02 : IExecutable
    {
        public void Execute()
        {
            var obj = new Sample();

            EventHandler handler = (s, e) => { Output.WriteLine("event raised."); };

            obj.TestEvent += handler;
            obj.FireEvents();
            obj.TestEvent -= handler;
            obj.FireEvents();
        }

        private class Sample
        {
            private readonly object _eventTarget = new object();

            public Sample()
            {
                Events = new EventHandlerList();
            }

            public EventHandlerList Events { get; }

            public event EventHandler TestEvent
            {
                add
                {
                    Output.WriteLine("add handler.");
                    Events.AddHandler(_eventTarget, value);
                }
                remove
                {
                    Output.WriteLine("remove handler.");
                    Events.RemoveHandler(_eventTarget, value);
                }
            }

            public void FireEvents()
            {
                var handler = Events[_eventTarget] as EventHandler;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}