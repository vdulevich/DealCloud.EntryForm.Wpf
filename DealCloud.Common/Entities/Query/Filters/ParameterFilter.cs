using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities.Query.Filters
{
    /// <summary>
    /// used as a placeholder, when we delay he actual filter value until later
    /// </summary>
    public class ParameterFilter
    {
        public object Value { get; set; }


        public override bool Equals(object obj)
        {
            var y = obj as ParameterFilter;
            return object.Equals(Value, y?.Value);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}
