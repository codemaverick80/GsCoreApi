using System;
using System.Collections.Generic;

namespace GsCore.Database.Data.Entities
{
    public partial class Cart
    {
        public Cart()
        {
            CartDetail = new HashSet<CartDetail>();
        }

        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<CartDetail> CartDetail { get; set; }
    }
}
