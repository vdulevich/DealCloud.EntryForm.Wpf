using System;
using System.Collections.Generic;
using System.Text;

namespace DealCloud.AddIn.Common.Utils
{
	public class Activity : IDisposable
	{
	    private static readonly List<Activity> current = new List<Activity>();

	    private readonly object[] tags;

		private Activity(object[] tags)
		{
			this.tags = tags;
		}

		void IDisposable.Dispose()
		{
			current.Remove(this);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach (object tag in tags)
			{
				sb.AppendFormat("{0}; ", tag);
			}

			return sb.ToString();
		}

		public static Activity Perform(params object[] tags)
		{
			if (tags.Length == 0)
			{
				throw new ArgumentNullException();
			}

			Activity activity = Find(tags);
			if (activity == null)
			{
				activity = new Activity(tags);
				current.Add(activity);
			}

			return activity;
		}

		public static bool IsInProcess(params object[] tags)
		{
			return Find(tags) != null;
		}

		private static Activity Find(object[] tags)
		{
			foreach (Activity activity in current)
			{
				if (activity.tags.Length == tags.Length)
				{
					int i;
					for (i = 0; i < tags.Length; i++)
					{
						if (!activity.tags[i].Equals(tags[i]))
						{
							break;
						}
					}
					if (i == tags.Length)
					{
						return activity;
					}
				}
			}

			return null;
		}
	}
}

