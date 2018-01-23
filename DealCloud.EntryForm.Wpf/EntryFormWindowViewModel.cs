using DealCloud.Common.Entities.AddInCommon;
using System.Collections.Generic;
using Telerik.Windows.Controls;

namespace Wpf_RichText
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<Field> _fields;
        public List<Field> Fields
        {
            get { return _fields; }
            set { _fields = value; OnPropertyChanged(nameof(Fields)); }
        }

        public MainWindowViewModel()
        {
            Fields = new List<Field>() {
                new Field() { Name = "Text", FieldType = DealCloud.Common.Enums.FieldTypes.Text },
                new Field() {  Name = "Choice",FieldType = DealCloud.Common.Enums.FieldTypes.Choice },
                new Field() {  Name = "Reference",FieldType = DealCloud.Common.Enums.FieldTypes.Reference }
            };
        }
    }
}
