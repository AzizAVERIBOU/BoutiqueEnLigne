using System.Collections.Generic;

namespace BoutiqueEnLigne.Models
{
    public class ApiResponse<T>
    {
        public List<T> Products { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
} 