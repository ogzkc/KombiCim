using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Data.Exceptions
{
    public enum RepositoryExceptionType
    {
        DeviceExist,
        ModeExist,
        LocationNotExist,
        MinTempNotExist
    }
}
