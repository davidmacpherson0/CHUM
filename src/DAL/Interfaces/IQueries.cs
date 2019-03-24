using Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IQueries
    {
        void LoadData();
        void CheckParamaters();

        List<object> GetQueryData();
        QueryBank GetQueryID();
    }
}
