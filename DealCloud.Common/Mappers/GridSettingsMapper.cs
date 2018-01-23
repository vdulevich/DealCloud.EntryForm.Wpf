using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealCloud.Common.Entities.Settings;

namespace DealCloud.Common.Mappers
{
    public class GridSettingsMapper<T> where T : class, new()

    {
        public GridSettings Map<TEntity>(GridSettings gridSettings)
        {
            var propNamesMapper = new PropertyNamesMapper<T>();
            var sortColumn = gridSettings.SortColumn;

            if (string.IsNullOrEmpty(sortColumn))
            {
                gridSettings.SortColumn = null;

            }
            else
            {
                var mappedPropName = propNamesMapper.MapName<TEntity>(gridSettings.SortColumn);
                //if provider uses anonymous object that doesn't match TEntity - make their property names identical :)
                if (mappedPropName == null)
                {
                    throw new ArgumentException($"Sort column '{sortColumn}' cannot be mapped correctly");
                }
                gridSettings.SortColumn = mappedPropName;
            }

            return gridSettings;
        }
    }
}
