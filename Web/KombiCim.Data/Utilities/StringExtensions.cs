using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kombicim.Data.Utilities
{
    public static class StringExtensions
    {
        public static bool NullEmpty(this string str) => string.IsNullOrEmpty(str);
    }
}
