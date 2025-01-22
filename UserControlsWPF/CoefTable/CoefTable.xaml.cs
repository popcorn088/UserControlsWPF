using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserControlsWPF.CoefTable
{
    /// <summary>
    /// CoefTable.xaml の相互作用ロジック
    /// </summary>
    public partial class CoefTable : UserControl
    {
        public static readonly DependencyProperty CoefsProperty
            = DependencyProperty.Register(
                "Coefs",
                typeof(Coefs),
                typeof(CoefTable),
                new PropertyMetadata(new Coefs()));
        public Coefs Coefs
        {
            get => (Coefs)this.GetValue(CoefsProperty);
            set => this.SetValue(CoefsProperty, value);
        }
        public static readonly DependencyProperty IndexHeaderProperty
            = DependencyProperty.Register(
                "IndexHeader",
                typeof(string),
                typeof(CoefTable),
                new PropertyMetadata("Index"));
        public string IndexHeader
        {
            get => (string)this.GetValue(IndexHeaderProperty);
            set => this.SetValue(IndexHeaderProperty, value);
        }
        public static readonly DependencyProperty ValueHeaderProperty
            = DependencyProperty.Register(
                "ValueHeader",
                typeof(string),
                typeof(CoefTable),
                new PropertyMetadata("Value"));
        public string ValueHeader
        {
            get => (string)this.GetValue(ValueHeaderProperty);
            set => this.SetValue(ValueHeaderProperty, value);
        }
        public static readonly DependencyProperty IndexVisibilityProperty
            = DependencyProperty.Register(
                "IndexVisibility",
                typeof(Visibility),
                typeof(CoefTable),
                new PropertyMetadata(Visibility.Visible));
        public Visibility IndexVisibility
        {
            get => (Visibility)this.GetValue(IndexVisibilityProperty);
            set => this.SetValue(IndexVisibilityProperty, value);
        }
        public static readonly DependencyProperty StringFormatProperty
            = DependencyProperty.Register(
                "StringFormat",
                typeof(string),
                typeof(CoefTable),
                new PropertyMetadata(string.Empty));
        public string StringFormat
        {
            get => (string)this.GetValue(StringFormatProperty);
            set => this.SetValue(StringFormatProperty, value);
        }
        public static readonly DependencyProperty TextAlignmentProperty
            = DependencyProperty.Register(
                "TextAlignment",
                typeof(TextAlignment),
                typeof(CoefTable),
                new PropertyMetadata(TextAlignment.Left));
        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }
        public CoefTable()
        {
            InitializeComponent();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            Coefs.Items.Add(new Coef()
            {
                Index = Coefs.Items.Count,
                Value = 0.0m
            });
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            List<Coef> selectedCoefs = new List<Coef>();
            foreach (Coef item in  Coefs.Items)
            {
                if (item.IsSelected)
                {
                    selectedCoefs.Add(item);
                }
            }
            for (int i = 0; i < selectedCoefs.Count; i++)
            {
                Coefs.Items.Remove(selectedCoefs[i]);
            }
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Coef item in e.AddedItems)
            {
                item.IsSelected = true;
            }
            foreach (Coef item in e.RemovedItems)
            {
                item.IsSelected = false;
            }
        }
    }
}
