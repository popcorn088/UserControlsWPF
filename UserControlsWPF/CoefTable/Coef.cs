namespace UserControlsWPF.CoefTable
{
    public class Coef : BindableBase
    {
        private int _index;
        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }
        private double _value;
        public double Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        public bool IsSelected { get; set; }
    }
}
