using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace UserControlsWPF.NumericUpDown
{
    /// <summary>
    /// NumericUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register(
                nameof(Value),
                typeof(decimal),
                typeof(NumericUpDown),
                new PropertyMetadata(0m));
        public decimal Value
        {
            get => (decimal)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }
        public static readonly DependencyProperty NickProperty
            = DependencyProperty.Register(
                nameof(Nick),
                typeof(decimal),
                typeof(NumericUpDown),
                new PropertyMetadata(1m));
        public decimal Nick
        {
            get => (decimal)this.GetValue(NickProperty);
            set => this.SetValue(NickProperty, value);
        }
        public static readonly DependencyProperty UpCommandProperty
            = DependencyProperty.Register(
                nameof(UpCommand),
                typeof(ICommand),
                typeof(NumericUpDown));
        public ICommand UpCommand
        {
            get => (ICommand)this.GetValue(UpCommandProperty);
            set => this.SetValue(UpCommandProperty, value);
        }
        public static readonly DependencyProperty DownCommandProperty
            = DependencyProperty.Register(
                nameof(DownCommand),
                typeof(ICommand),
                typeof(NumericUpDown));
        public ICommand DownCommand
        {
            get => (ICommand)this.GetValue(DownCommandProperty);
            set => this.SetValue(DownCommandProperty, value);
        }
        public static readonly DependencyProperty MaximumProperty
            = DependencyProperty.Register(
                nameof(Maximum),
                typeof(decimal),
                typeof(NumericUpDown),
                new PropertyMetadata(100m));
        public decimal Maximum
        {
            get => (decimal)this.GetValue(MaximumProperty);
            set => this.SetValue(MaximumProperty, value);
        }
        public static readonly DependencyProperty MinimumProperty
            = DependencyProperty.Register(
                nameof(Minimum),
                typeof(decimal),
                typeof(NumericUpDown),
                new PropertyMetadata(0m));
        public decimal Minimum
        {
            get => (decimal)this.GetValue(MinimumProperty);
            set => this.SetValue(MinimumProperty, value);
        }
        public static readonly DependencyProperty StringFormatProperty
            = DependencyProperty.Register(
                nameof(StringFormat),
                typeof(string),
                typeof(NumericUpDown),
                new PropertyMetadata(string.Empty));
        public string StringFormat
        {
            get => (string)this.GetValue(StringFormatProperty);
            set => this.SetValue(StringFormatProperty, value);
        }
        public NumericUpDown()
        {
            InitializeComponent();
            UpCommand = new RelayCommand(UpCommandExecute);
            DownCommand = new RelayCommand(DownCommandExecute);
        }

        private void DownCommandExecute()
        {
            if (Value - Nick >= Minimum)
            {
                Value -= Nick;
            }
        }

        private void UpCommandExecute()
        {
            if (Value + Nick <= Maximum)
            {
                Value += Nick;
            }
        }
    }

    public class DecimalConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is decimal dec)
            {
                return dec.ToString((string)values[1]);
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
