using Common.Data.Logging;
using Common.Events;
using Common.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class Log : ILog
    {
        private IEventAggregator _eventAggregator;

        public void LogMessage(string Message)
        {
            this._eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this._eventAggregator.GetEvent<LogMessageEvent>().Publish(new LogMessage(DateTime.Now, Message));
        }
    }
}
