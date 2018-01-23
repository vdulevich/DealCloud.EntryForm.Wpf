using DealCloud.Common.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Wpf_RichText.Features.EntryForm.Fields
{
    public partial class EntryFieldSelect : UserControl
    {
        public int ListId { get; set; }

        public int FieldId { get; set; }

        public ObservableCollection<NamedEntry> Items { get; set; }

        private int[] _value;
        public int[] Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }

        public EntryFieldSelect()
        {
            InitializeComponent();
            Items = new ObservableCollection<NamedEntry>();
            Items.Add(new NamedEntry() { Name = "Text" });
            Items.Add(new NamedEntry() { Name = "Text1" });
            Items.Add(new NamedEntry() { Name = "Text2" });
        }
    }
}
