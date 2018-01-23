using System;
using System.Configuration;
using DealCloud.Common.Extensions;

namespace DealCloud.AddIn.Common.Utils
{
    public class AddInConfigSection : ConfigurationSection, ICloneable
    {
        private const string AddInConfigSectionName = "addinSettings";

        [ConfigurationProperty("Logout", IsRequired = false)]
        public LogoutConfigurationElement Logout => (LogoutConfigurationElement)(base["Logout"]);

        [ConfigurationProperty("SsoConfig", IsRequired = true)]
        public SsoConfigurationElement SsoConfig => (SsoConfigurationElement)(base["SsoConfig"]);

        [ConfigurationProperty("QueryBuilderConfig")]
        public QueryBuilderConfigurationElement QueryBuilderConfig => (QueryBuilderConfigurationElement)(base["QueryBuilderConfig"]);

        [ConfigurationProperty("DataCenters", IsRequired = false)]
        public DataCentersCollection DataCenters => (DataCentersCollection)(base["DataCenters"]);

        public static AddInConfigSection Read()
        {
            return (AddInConfigSection)ConfigurationManager.GetSection(AddInConfigSectionName);
        }

        public void Save()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Remove(AddInConfigSectionName);
            config.Sections.Add(AddInConfigSectionName, (AddInConfigSection)Clone());
            config.Save();
        }

        public void SetDataCenter(string dataCenterName)
        {
            if (dataCenterName.IsNullOrEmpty()) throw new ArgumentException($"Parameter {nameof(dataCenterName)} can't be null or empty");
            var newDataCenter = DataCenters[dataCenterName];
            if (newDataCenter != null)
            {
                SsoConfig.Url = newDataCenter.Url;
                Save();
            }
        }

        public object Clone()
        {
            AddInConfigSection config = new AddInConfigSection
            {
                ["Logout"] = this.Logout,
                ["SsoConfig"] = this.SsoConfig,
                ["QueryBuilderConfig"] = this.QueryBuilderConfig,
                ["DataCenters"] = this.DataCenters
            };
            return config;
        }
    }

    public class LogoutConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public bool Value
        {
            get { return ((bool)base["value"]); }
            set { base["value"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class SsoConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return ((string)base["url"]); }
            set { base["url"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class QueryBuilderConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get { return ((string)(base["path"])); }
            set { base["path"] = value; }
        }
    }

    [ConfigurationCollection(typeof(DataCenterElement), AddItemName = "DataCenter")]
    public class DataCentersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DataCenterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DataCenterElement)element).Name;
        }

        public DataCenterElement this[int index] => (DataCenterElement)BaseGet(index);

        public DataCenterElement this[string key] => (DataCenterElement)BaseGet(key);
    }

    public class DataCenterElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)(base["name"])); }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return ((string)(base["url"])); }
            set { base["url"] = value; }
        }

        [ConfigurationProperty("version")]
        public string Version
        {
            get { return ((string)(base["version"])); }
            set { base["version"] = value; }
        }
    }
}
