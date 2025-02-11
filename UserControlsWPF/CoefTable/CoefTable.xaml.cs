﻿using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UserControlsWPF.CoefTable
{
    /// <summary>
    /// CoefTable.xaml の相互作用ロジック
    /// </summary>
    public partial class CoefTable : UserControl
    {
        public static readonly DependencyProperty CoefsProperty
            = DependencyProperty.Register(
                nameof(Coefs),
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
                nameof(IndexHeader),
                typeof(string),
                typeof(CoefTable),
                new PropertyMetadata(nameof(IndexHeader)));

        public string IndexHeader
        {
            get => (string)this.GetValue(IndexHeaderProperty);
            set => this.SetValue(IndexHeaderProperty, value);
        }
        public static readonly DependencyProperty ValueHeaderProperty
            = DependencyProperty.Register(
                nameof(ValueHeader),
                typeof(string),
                typeof(CoefTable),
                new PropertyMetadata(nameof(ValueHeader)));
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
        public static readonly DependencyProperty IsReadOnlyProperty
            = DependencyProperty.Register(
                nameof(IsReadOnly),
                typeof(bool),
                typeof(CoefTable),
                new PropertyMetadata(false));
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
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
                Value = 0.0,
            });
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            var selectedCoefs = new List<Coef>();
            foreach (Coef item in Coefs.Items)
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
            for (int i = 0; i < Coefs.Items.Count; i++)
            {
                Coefs.Items[i].Index = i++;
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

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not CoefTable coefTable)
            {
                return Visibility.Visible;
            }
            if (coefTable.IsReadOnly)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
