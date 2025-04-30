using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    // Valueの範囲Validationチェックは以下を参考にした
    // https://sourcechord.hatenablog.com/entry/2014/06/08/123738

    /// <summary>
    /// NumericUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyDataErrorInfo
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
            set
            {
                ValidateProperty("Value", value);
                if (!HasErrors)
                {
                    this.SetValue(ValueProperty, value);
                }
            }
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

        readonly Dictionary<string, List<string>> _currentErrors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public string StringFormat
        {
            get => (string)this.GetValue(StringFormatProperty);
            set => this.SetValue(StringFormatProperty, value);
        }

        public bool HasErrors => _currentErrors.Count > 0;

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

        protected void ValidateProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "Value":
                    if (value is decimal dec)
                    {
                        if (!((Minimum <= dec) && (dec <= Maximum)))
                        {
                            AddError("Value", "out of range.");
                        }
                        else
                        {
                            RemoveError("Value");
                        }
                    }
                    else
                    {
                        AddError("Value", "not decimal.");
                    }
                    break;
                default:
                    break;
            }
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_currentErrors.ContainsKey(propertyName))
            {
                _currentErrors[propertyName] = new List<string>();
            }

            if (!_currentErrors[propertyName].Contains(error))
            {
                _currentErrors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void RemoveError(string propertyName)
        {
            if (_currentErrors.ContainsKey(propertyName))
            {
                _currentErrors.Remove(propertyName);
            }

            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            var h = this.ErrorsChanged;
            if (h != null)
            {
                h(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) ||
                !_currentErrors.ContainsKey(propertyName))
            {
                return null;
            }

            return _currentErrors[propertyName];
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
