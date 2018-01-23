using System;
using System.Collections.Generic;
using System.Linq;

namespace DealCloud.Common.Extensions
{
    public static class TypeExtension
    {
        public static Type IsA(this Type type, Type typeToBe)
        {
            if (!typeToBe.IsGenericTypeDefinition && typeToBe.IsAssignableFrom(type))
            {
                return typeToBe;
            }

            var toCheckTypes = new List<Type> { type };
            if (typeToBe.IsInterface)
                toCheckTypes.AddRange(type.GetInterfaces());

            var basedOn = type;
            while (basedOn.BaseType != null)
            {
                toCheckTypes.Add(basedOn.BaseType);
                basedOn = basedOn.BaseType;
            }
            return toCheckTypes.SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeToBe);
        }
    }
}
