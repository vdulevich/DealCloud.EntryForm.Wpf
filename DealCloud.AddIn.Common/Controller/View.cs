using System;
using System.Collections.Generic;
using System.Linq;
using DealCloud.AddIn.Common.Commands;

namespace DealCloud.AddIn.Common.App
{
    public abstract class View<TContext> where TContext :class 
    {
        public TContext CurrentContext { get; set; }

        public View<TContext> Parent { get; set; }

        public IEnumerable<View<TContext>> Views => _views;

        private readonly List<View<TContext>> _views = new List<View<TContext>>();

        protected View<TContext> FindParentView(Type type)
        {
            return this.Parent?.GetType() == type ? this.Parent : this.Parent?.FindParentView(type);
        }

        public void AddView(View<TContext> view)
        {
            _views.Add(view);
            view.CurrentContext = view.CurrentContext ?? this.CurrentContext;
            view.Parent = this;
            view.Init();
        }

        public void RemoveView(View<TContext> view)
        {
            _views.Remove(view);
            view.Parent = null;
            view.Close();
        }

        protected abstract void Update(ICommand commands);

        internal void DoUpdate(ICommand command)
        {
            Update(command);
            foreach (var view in Views.ToList())
            {
                view.DoUpdate(command);
            }
        }

        internal void DoUpdate(IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
            {
                DoUpdate(command);
            }
        }

        protected virtual void Init()
        {

        }

        public virtual void Close()
        {
            foreach (var view in Views.ToList())
            {
                RemoveView(view);
            }
        }
    }
}
