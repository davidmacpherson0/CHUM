using Common.Base;
using Common.Data.Logging;
using Common.Events;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class LogViewModel : BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        private ObservableCollection<LogMessage> _ListOfMessages;


        public LogViewModel()
        {
            this.ListOfMessages = new ObservableCollection<LogMessage>();
            this.ListOfMessages.Add(new LogMessage(DateTime.Now, "Ready to Go please press pre-checks"));
            this._eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this._eventAggregator.GetEvent<LogMessageEvent>().Subscribe(AddMessage);
        }


        private void AddMessage(LogMessage message)
        {
            UIInvoke(() =>
            {
                this.ListOfMessages.Add(message);
                this.ListOfMessages = new ObservableCollection<LogMessage>(from mess in this.ListOfMessages
                                                                           orderby mess.Date descending
                                                                           select mess);

            });
        }



        public ObservableCollection<LogMessage> ListOfMessages
        {
            get { return _ListOfMessages; }
            set
            {
                _ListOfMessages = value;
                base.NotifyPropertyChanged("ListOfMessages");
            }
        }





    }
}
