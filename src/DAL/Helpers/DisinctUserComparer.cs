using DAL.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public class DisinctUserComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(User obj)
        {
            return obj.GetHashCode();
        }
    }
}
