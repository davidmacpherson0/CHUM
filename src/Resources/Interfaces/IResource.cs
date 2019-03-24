using Common.Constants;
using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Interfaces
{
    public interface IResource
    {
        void InitialiseResource();
        ResourceName getName();
    }
}
