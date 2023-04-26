using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Models.QueryParameters
{
    public class ContestQueryParameter
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Phase { get; set; }
        public string Type { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 2;

    }
}
