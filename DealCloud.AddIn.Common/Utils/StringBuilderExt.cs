using System.Collections.Generic;
using System.Text;

namespace DealCloud.AddIn.Common.Utils
{
    public static class StringBuilderExt
    {
        public static StringBuilder AppendLines(this StringBuilder sb, IEnumerable<object> lines)
        {
            foreach (var line in lines)
            {
                sb.AppendLine(line?.ToString());
            }
            return sb;
        }
    }
}
