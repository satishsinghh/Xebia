using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.DatabaseCore.Common
{
    public class PagingUtility 
    {
        public static int CalculateStartRecord(int currentPage, int recordsPerPage)
        {
            return (currentPage <= 1) ? 0 : (currentPage - 1) * recordsPerPage;
        }

        public static int CalculateCurrentPage(int totalPages, int totalRecords, int startRecord, int recordsPerPage, int @base = 1)
        {
            var currentPage = @base;

            if (startRecord > 0)
            {
                var recordsRemaining = totalRecords - startRecord;
                var pagesRemaining = recordsRemaining / recordsPerPage;

                currentPage = totalPages - pagesRemaining;

                if (totalRecords % recordsPerPage == 0)
                {
                    currentPage += 1;
                }
            }

            return currentPage;
        }

        public static int CalculateTotalPages(int totalRecords, int recordsPerPage)
        {
            var totalPages = 0;

            if (recordsPerPage > 0)
            {
                var pageOverflow = ((totalRecords % recordsPerPage) > 0) ? 1 : 0;
                totalPages = (totalRecords / recordsPerPage) + pageOverflow;
            }

            return totalPages;
        }
    }
}
