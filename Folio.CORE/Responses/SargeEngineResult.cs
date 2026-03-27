using System;
using System.Collections.Generic;
using System.Text;

namespace Folio.CORE.Responses
{
    public class SargeEngineResult<T>
    {
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int PageTotal { get; }
        public IEnumerable<T> Values { get; }
        public SargeEngineResult(int currentPage, int pageSize, int pageTotal, IEnumerable<T> values)
        {
            PageTotal = pageTotal;
            PageSize = pageSize;
            CurrentPage = currentPage >= PageTotal ? PageTotal : currentPage;
            this.Values = values;
        }
    }
}
