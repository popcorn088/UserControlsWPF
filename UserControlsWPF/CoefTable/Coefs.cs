﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace UserControlsWPF.CoefTable
{
    public class Coefs : BindableBase
    {
        private ObservableCollection<Coef> _items = [];
        public ObservableCollection<Coef> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public delegate void CoefValueChangedDelegate(Coef? coef);
        public CoefValueChangedDelegate? CoefValueChanged;
        public delegate void NumOfCoefsChangedDelegate();
        public NumOfCoefsChangedDelegate? NumOfCoefsChanged;
        public Coefs() => _items.CollectionChanged += ValueCollectionChanged;

        private void ValueCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (Coef item in e.NewItems)
                    {
                        item.PropertyChanged += ItemPropertyChanged;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (Coef item in e.OldItems)
                    {
                        item.PropertyChanged -= ItemPropertyChanged;
                    }
                }
            }

            NumOfCoefsChanged?.Invoke();
        }

        private void ItemPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
            => CoefValueChanged?.Invoke(sender as Coef);
    }
}
