using System;

namespace DealCloud.AddIn.Common.Commands
{
    public interface ICommand
    {
        bool NotifyViews { get; set; }

        Object Sender { get; set; }

        bool NeedProcess { get; }

        void Process();
    }
}
