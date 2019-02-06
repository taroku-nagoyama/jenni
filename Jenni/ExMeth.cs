using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenni
{
    public static class ExMeth
    {
        public static IEnumerable<T[]> Paginazu<T>(this IEnumerable<T> list, int pageSize)
        {
            var accum = new List<T>();
            int i = 0;
            foreach (var item in list)
            {
                accum.Add(item);
                i++;
                if (i % pageSize == 0)
                {
                    yield return accum.ToArray();
                    accum.Clear();
                }
            }
            if (i % pageSize != 0)
                yield return accum.ToArray();
        }
    }
}
