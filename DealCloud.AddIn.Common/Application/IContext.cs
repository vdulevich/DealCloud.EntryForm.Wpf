using DealCloud.AddIn.Common.App;

namespace DealCloud.AddIn.Common
{
    public interface IContext<TModel, TContext> where TContext : class
    {
        Controller<TContext> Controller { get; }

        TModel Model { get; }
    }
}
