using System.Text;

namespace DealCloud.AddIn.Common.Utils
{
    public static class RibbonExt
    {
        public static string GetDataCenterMenuContent(DataCentersCollection dataCenters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<menu xmlns='http://schemas.microsoft.com/office/2006/01/customui'>");
            foreach (DataCenterElement dataCenter in dataCenters)
            {
                sb.Append($"<button id='{dataCenter.Name}' imageMso='DatabaseCopyDatabaseFile' onAction='OnAuthAction' label='{dataCenter.Name}' />");
            }
            sb.Append("</menu>");
            return sb.ToString();
        }
    }
}
