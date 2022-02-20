using System;
using System.Collections.Generic;

namespace Shop.Core.Model
{
    public class PageModel<T>
    {
        public int Page { get; set; }

        public int PageCount { get; set; }

        public int DataCount { get; set; }

        public int PageSize { get; set; }

        public List<T> Data { get; set; }
    }
}
