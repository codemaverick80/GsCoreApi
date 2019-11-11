using System;
using System.Collections.Generic;

namespace GsCore.Database.Data.Entities
{
    public partial class CartDetail
    {
        public int Id { get; set; }
        public Guid? CartId { get; set; }
        public Guid? InventoryId { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
