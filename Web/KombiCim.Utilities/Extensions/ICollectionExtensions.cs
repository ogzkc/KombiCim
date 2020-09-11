using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KombiCim.Utilities.Extensions
{
    public static class ICollectionExtensions
    {
        public static bool AnyNull<T>(this ICollection<T> list) => list != null && list.Any();
    }
}
