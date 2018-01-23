using System;
using System.Collections.Generic;
using System.Linq;

namespace DealCloud.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyNamesAttribute : Attribute
    {
        #region [Properties]

        public List<string> PropertyNames { get; }

        #endregion

        #region [Constructors]

        public PropertyNamesAttribute()
        {

            PropertyNames = new List<string>();
        }

        public PropertyNamesAttribute(params string[] propertyNames)
        {

            PropertyNames = propertyNames.ToList();
        }

        #endregion
    }
}
