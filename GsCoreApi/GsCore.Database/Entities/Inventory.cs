using System;
using System.Collections.Generic;

namespace GsCore.Database.Entities
{
    public partial class Inventory
    {
        public Guid Id { get; set; }
        public Guid AlbumId { get; set; }
        public int? QtyAvailable { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }

        public virtual Album Album { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }
    }
}
