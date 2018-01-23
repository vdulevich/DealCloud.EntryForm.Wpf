using DealCloud.Common.Entities.Settings;
using System;

namespace DealCloud.Common.Validators 
{
    public class GridSettingsValidator : IValidator<GridSettings>
    {
        public bool Validate(GridSettings gridSettings)
        {
            if (gridSettings.PageIndex < 1)
            {
                throw new ArgumentException($"{nameof(gridSettings.PageIndex)} can not be less 1.", nameof(gridSettings.PageIndex));
            }
            if (gridSettings.PageSize < 1)
            {
                throw new ArgumentException($"{nameof(gridSettings.PageSize)} can not be less 1.", nameof(gridSettings.PageSize));
            }

            return true;
        }
    }
}
