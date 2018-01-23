using DealCloud.AddIn.Common.App;

namespace DealCloud.AddIn.Common.Commands
{
    public abstract class CommandResult<TContext, TResult> : Command<TContext>
    {
        public TResult Result { get; protected set; }

        public CommandResult() { }

        public CommandResult(object control):base(control)
        {

        }

        public CommandResult(Command<TContext> command)
        {
            Sender = command.Sender;
            //BulkUpdate = command.BulkUpdate;
            CurrentContext = command.CurrentContext;
        }
    }
}
