using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.DatabaseCore.Common 
{
    public class PagedList<T> : List<T>
    {
        public int TotalRecords { get; private set; }
        public int TotalPages { get; private set; }
        public int RecordsPerPage { get; private set; }
        public int CurrentPage { get; private set; }

        public PagedList(IEnumerable<T> collection, int totalRecords, int startRecord, int recordsPerPage)
            : base(collection)
        {
            this.TotalRecords = totalRecords;
            this.RecordsPerPage = recordsPerPage;
            this.TotalPages = PagingUtility.CalculateTotalPages(this.TotalRecords, this.RecordsPerPage);
            this.CurrentPage = PagingUtility.CalculateCurrentPage(this.TotalPages, this.TotalRecords, startRecord, this.RecordsPerPage);
        }
    }
}
