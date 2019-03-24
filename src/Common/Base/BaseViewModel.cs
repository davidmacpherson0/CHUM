using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ClosingRequest;

        protected void NotifyPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                System.Diagnostics.Debug.WriteLine("Property Name Changed: " + PropertyName);
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        protected void OnClosingRequest()
        {
            if (this.ClosingRequest != null)
            {
                this.ClosingRequest(this, EventArgs.Empty);
            }
        }
        protected void UIInvoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

    }
}


