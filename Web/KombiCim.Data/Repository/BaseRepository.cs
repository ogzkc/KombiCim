using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Repository
{
    public class BaseRepository
    {
        protected static DateTime Now
        {
            get
            {
                var now = DateTime.Now;
                return now.AddMilliseconds(-now.Millisecond);
            }
        }
    }
}
