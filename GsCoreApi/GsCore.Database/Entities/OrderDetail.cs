using System;

namespace GsCore.Database.Entities
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid? InventoryId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Order Order { get; set; }
    }
}
