using Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class ResourcePayload
    {
        private ResourceName _Name;
        private object _Data;

        public ResourcePayload(ResourceName name)
        {
            this.Name = name;
        }

        public object Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        public ResourceName Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

    }
}
