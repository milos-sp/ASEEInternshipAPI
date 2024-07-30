using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using System.Xml.Linq;

namespace ProductAPI.Objects
{
    public class QueryObject
    {
        [BindProperty(Name = "transaction-kind")]
        public string? TransactionKind { get; set; } // moze vise da ih bude, razdvojeni zarezom

        [BindProperty(Name = "start-date")]
        public DateTime? StartDate { get; set; }

        [BindProperty(Name = "end-date")]
        public DateTime? EndDate { get; set; }

        [BindProperty(Name = "page")]
        public int Page { get; set; } = 1;

        [BindProperty(Name = "page-size")]
        public int PageSize { get; set; } = 10;

        [BindProperty(Name = "sort-by")]
        public string? SortBy { get; set; } = null;

        [BindProperty(Name = "sort-order")]
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;
    }
}