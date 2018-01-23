using System;

namespace DealCloud.Common.Cache
{
    [Serializable]
    public class TimeoutReference<T>
    {
        private readonly TimeSpan _timeout;

        private readonly bool _updatable;

        private DateTime _updated;

        public DateTime Created { get; }

        public T Target { get; private set; }

        public bool IsAlive
        {
            get
            {
                if (DateTime.UtcNow - _updated > _timeout) return false;

                Update();

                return true;
            }
        }

        public TimeoutReference(T target, TimeSpan timeout)
        {
            Target = target;
            _timeout = timeout;
            _updatable = false;
            _updated = Created = DateTime.UtcNow;
        }

        public TimeoutReference(T target, TimeSpan timeout, bool updatable)
        {
            Target = target;
            _timeout = timeout;
            _updatable = updatable;
            _updated = Created = DateTime.UtcNow;
        }

        internal void SetTarget(T target)
        {
            Target = target;

            Update();
        }


        public void Update()
        {
            if (_updatable)
            {
                _updated = DateTime.UtcNow;
            }
        }
    }
}