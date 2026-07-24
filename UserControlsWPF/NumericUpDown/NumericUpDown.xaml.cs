using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
                new FrameworkPropertyMetadata(
                    0m,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (NumericUpDown)d;
            c.ValidateProperty(nameof(Value), e.NewValue);
        }

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

        readonly Dictionary<string, List<string>> _currentErrors = [];
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
                case nameof(Value):
                    if (value is decimal dec)
                    {
                        if (!((Minimum <= dec) && (dec <= Maximum)))
                        {
                            AddError(nameof(Value), "out of range.");
                        }
                        else
                        {
                            RemoveError(nameof(Value));
                        }
                    }
                    else
                    {
                        AddError(nameof(Value), "not decimal.");
                    }
                    break;
                default:
                    break;
            }
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_currentErrors.TryGetValue(propertyName, out List<string>? value))
            {
                value = [];
                _currentErrors[propertyName] = value;
            }

            if (!value.Contains(error))
            {
                value.Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void RemoveError(string propertyName)
        {
            _currentErrors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) ||
                !_currentErrors.TryGetValue(propertyName, out List<string>? value))
            {
                return new List<string>();
            }

            return value;
        }
    }

    public class DecimalConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(v => v == DependencyProperty.UnsetValue))
            {
                return Binding.DoNothing;
            }

            if (values[0] is decimal dec && values[1] is string format)
            {
                return string.IsNullOrEmpty(format) ? dec.ToString() : dec.ToString(format);
            }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is string str && decimal.TryParse(str, NumberStyles.Any, culture, out decimal dec))
            {
                return [dec, Binding.DoNothing];
            }
            return [DependencyProperty.UnsetValue, Binding.DoNothing];
        }
    }
}
