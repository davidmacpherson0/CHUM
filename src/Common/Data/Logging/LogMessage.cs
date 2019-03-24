using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Logging
{
    public class LogMessage
    {
        private string _Message;
        private DateTime _Date;

        public LogMessage(DateTime Date, string Message)
        {
            this.Date = Date;
            this.Message = Message;
        }

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

    }
}
