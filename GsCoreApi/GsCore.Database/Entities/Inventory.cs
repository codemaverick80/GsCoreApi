using System;
using System.Collections.Generic;

namespace GsCore.Database.Entities
{
    public partial class Inventory
    {
        public Inventory()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public Guid Id { get; set; }
        public int AlbumId { get; set; }
        public int? QtyAvailable { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }

        public virtual Album Album { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
