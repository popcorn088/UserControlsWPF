using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserControlsWPF.CoefTable
{
    public class CoefTableViewModel : BindableBase
    {
        private Coefs _coefs = new Coefs();
        public Coefs Coefs
        {
            get => _coefs;
            set => SetProperty(ref _coefs, value);
        }
        public CoefTableViewModel()
        {
            Coefs.Items.Add(new Coef()
            {
                Value = 2,
            });
        }
    }
}
