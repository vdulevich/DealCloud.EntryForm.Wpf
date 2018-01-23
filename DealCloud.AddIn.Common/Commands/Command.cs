using System;
using DealCloud.AddIn.Common.Commands;

namespace DealCloud.AddIn.Common.App
{
    public abstract class Command<TContext>: ICommand
    {
        public TContext CurrentContext { get; set; }

        public virtual bool NotifyViews { get; set; }

        public Object Sender { get; set; }

        public virtual bool NeedProcess
        {
            get { return true; }
        }

        public Command() { }

        public Command(Command<TContext> command)
        {
            Sender = command.Sender;
            CurrentContext = command.CurrentContext;
	        NotifyViews = command.NotifyViews;
        }

        public Command(Object sender)
        {
            Sender = sender;
        }

        public abstract void Process();
    }
}
