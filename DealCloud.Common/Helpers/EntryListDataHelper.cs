using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealCloud.Common.Entities;

namespace DealCloud.Common.Helpers
{
    public static class EntryListDataHelper
    {
        public static IEnumerable<NamedEntry> GetEntriesFromValue(object value)
        {
            if (value is NamedEntry)
                yield return value as NamedEntry;
            else
            {
                var l = value as IList;
                if (l != null)
                {
                    foreach (var o in l)
                        yield return o as NamedEntry;
                }
            }
        }
    }
}
