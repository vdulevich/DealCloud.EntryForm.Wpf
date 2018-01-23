using System;

namespace DealCloud.AddIn.Common.Utils
{
    public static class VbaUtil
    {
        public static bool IsVbaInstalled(string productCode)
        {
            Type installerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            dynamic installer = Activator.CreateInstance(installerType);
            return installer.FeatureState(productCode, "VBAFiles") == 3;
        }
    }
}
