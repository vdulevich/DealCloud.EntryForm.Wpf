using System;

namespace DealCloud.Common.Cache
{
    [Serializable]
    internal class VersionedTimeoutReference<T> : TimeoutReference<T>
    {
        public long Version { get; private set; }

        public VersionedTimeoutReference(T target, TimeSpan timeout, long version = 0) : base(target, timeout)
        {
            Version = version;
        }

        public VersionedTimeoutReference(T target, TimeSpan timeout, bool updatable, long version = 0) : base(target, timeout, updatable)
        {
            Version = version;
        }

        internal void SetTarget(T target, long version)
        {
            Version = version;

            SetTarget(target);
        }
    }
}