using System;
using System.Collections.Generic;

namespace Shopping.Models
{
    public partial class ProductLike
    {
        public int ProductLikeId { get; set; }
        public int? CustomerID { get; set; }
        public int? ProductId { get; set; }
        public byte? IsLiked { get; set; }
        public int? ActionCount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }
    }
}
