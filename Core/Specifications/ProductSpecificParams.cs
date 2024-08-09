using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductSpecificParams
    {
        public int? ProductBrandId { set; get; }
        public int? ProductTypeId { set; get; }
        public string? Sort { get; set; }
        private const int MAXPAGESIZE = 60;
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set =>_pageSize= value > MAXPAGESIZE ? MAXPAGESIZE : value;
        }
        public int PageIndex { get; set; } = 1;
        private string? _search;

        public string? Search
        {
            get =>  _search; 
            set => _search = value.ToLower(); 
        }


    }
}
