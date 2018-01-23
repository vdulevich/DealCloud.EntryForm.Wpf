using System.Collections.Generic;
using System.Linq;
using DealCloud.AddIn.Common.Commands;
using System.Windows.Threading;

namespace DealCloud.AddIn.Common.App
{
    internal class RootView<TContext> : View<TContext> where TContext: class 
    {
        protected override void Update(ICommand commands)
        {

        }
    }

    public class Controller<TContext> where TContext : class
    {
        public Dispatcher Dispatcher { get; set; }

        public TContext Context { get; set; }

        public View<TContext> View { get; }

        private readonly object _syncObject = new object();

        public Controller()
        {
            View = new RootView<TContext>();
        }

        public Controller(View<TContext> rootView)
        {
            View = rootView;
        }

        public void ExecuteCommand(Command<TContext> command)
        {
            ExecuteCommandInternal(command);
        }

        private void ExecuteCommandInternal(Command<TContext> command)
        {
            command.CurrentContext = Context;
            if (command.NeedProcess)
            {
                command.Process();
                if (command.NotifyViews)
                {
                    if (Dispatcher?.CheckAccess() ?? true)
                    {
                        UpdateViews(command);
                    }
                    else
                    {
                        Dispatcher?.Invoke(() => { UpdateViews(command); });
                    }
                    
                }
            }
        }

        public void UpdateViews(ICommand command)
        {
            lock (_syncObject)
            {
                View.DoUpdate(command);
            }
        }

        public void RemoveView(TContext context)
        {
            lock (_syncObject)
            {
                foreach (View<TContext> view in View.Views.Where(pred => pred.CurrentContext?.Equals(context) == true).ToList())
                {
                    View.RemoveView(view);
                }
            }
        }
    }
}
