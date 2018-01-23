using DealCloud.Common.Entities.AddInCommon;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DealCloud.EntryForm.Wpf
{
    public partial class EntryFormComponent : UserControl
    {
        public static readonly DependencyProperty FieldsProperty = DependencyProperty.Register(
            "ListId", 
            typeof(int), 
            typeof(EntryFormComponent), 
            new PropertyMetadata(-1, ListIdPropertyChanged));

        private static void ListIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryFormComponent entryForm = d as EntryFormComponent;
            entryForm.InitializaControls();
        }

        public int ListId
        {
            get { return (int)GetValue(FieldsProperty); }
            set { SetValue(FieldsProperty, value); }
        }

        public EntryFormComponent()
        {
            InitializeComponent();
            InitializaControls();
        }

        private void InitializaControls()
        {
            LayoutRoot.Children.Clear();
            LayoutRoot.RowDefinitions.Clear();
            foreach (Field field in (DataContext as EntryFormViewModel).Fields)
            {
                LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                Label label = new Label() { Content = field.Name, Style = (Style)Resources["nameStyle"] };
                Grid.SetRow(label, LayoutRoot.RowDefinitions.Count - 1);
                LayoutRoot.Children.Add(label);
                label = new Label() { Content = $"Add {field.Name}", HorizontalAlignment = HorizontalAlignment.Right };
                Grid.SetRow(label, LayoutRoot.RowDefinitions.Count - 1);
                LayoutRoot.Children.Add(label);

                LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                UIElement control = GetFromField(field);
                Grid.SetRow(control, LayoutRoot.RowDefinitions.Count - 1);
                LayoutRoot.Children.Add(control);
            }
        }

        private UIElement GetFromField(Field field)
        {
            switch (field.FieldType)
            {
                case DealCloud.Common.Enums.FieldTypes.Text:
                    return new TextBox() { DataContext = field };
                case DealCloud.Common.Enums.FieldTypes.Reference:
                    return new Fields.EntryFieldSelect() { DataContext = field };
                case DealCloud.Common.Enums.FieldTypes.Choice:
                    return new CheckBox() { DataContext = field };
                default:
                    return new Control();
            }
        }
    }
}
