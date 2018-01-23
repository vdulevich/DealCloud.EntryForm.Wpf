using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace DealCloud.Common.Serialization.Binders
{
    public class KnownTypesBinder: SerializationBinder
    {
        public IList<Type> Types { get; set; }

        public override Type BindToType(string assemblyName, string typeName)
        {
            var type = Types.SingleOrDefault(t => t.FullName == typeName);
            if (type != null)
                return type;
            return Assembly.Load(assemblyName).GetType(typeName);
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.AssemblyQualifiedName;
            typeName = serializedType.Name;
        }
    }
}
