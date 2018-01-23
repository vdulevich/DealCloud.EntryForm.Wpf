using System;

namespace DealCloud.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SanitizeAttribute : Attribute
    {
    }
}