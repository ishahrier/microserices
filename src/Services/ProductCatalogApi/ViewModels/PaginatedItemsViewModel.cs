using System.Collections.Generic;

namespace ProductCatalogApi.ViewModels {
    public class PaginatedItemsViewModel<T> where T : class {
        public PaginatedItemsViewModel (int pageIndex, int pageSize, long count, IEnumerable<T> data) {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }
        public int PageSize { get; private set; }
        public int PageIndex { get; private set; }
        public long Count { get; private set; }
        public IEnumerable<T> Data { get; set; }
    }
}