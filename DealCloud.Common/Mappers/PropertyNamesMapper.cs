using System;
using System.Linq;
using System.Reflection;
using DealCloud.Common.Attributes;

namespace DealCloud.Common.Mappers
{
    public class PropertyNamesMapper<T> where T : class, new()
    {
        #region [Methods]

        public string MapName<TEntity>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var attr = (PropertyNamesAttribute) typeof(T).GetProperty(ConvertFirstCharToUpper(name))
                ?.GetCustomAttribute(typeof(PropertyNamesAttribute));
            if (attr != null)
            {
                var mappingPropName = typeof(TEntity).GetProperties().FirstOrDefault(p => attr.PropertyNames.Contains(p.Name))?.Name;
                return mappingPropName;
            }

            return null;
        }

        private static string ConvertFirstCharToUpper(string input)
        {                
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        #endregion
    }
}
