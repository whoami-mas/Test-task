using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Plane.Model
{
    public class Information : INotifyPropertyChanged
    {
        private string _info;

        public string Info
        {
            get { return _info; }
            set { _info = value;
                OnPropertyChanged("Info");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string name = null)
        {
            if(PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(name)); }
        }
    }
}
