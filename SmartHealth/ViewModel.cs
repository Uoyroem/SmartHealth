using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealth
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PropertyChangedEventHandler Redirector()
        {
            return (s, e) =>
            {
                GetType().GetProperty(e.PropertyName!)!.SetValue(this, s!.GetType().GetProperty(e.PropertyName!)!.GetValue(s));
            };
        }
}
}
