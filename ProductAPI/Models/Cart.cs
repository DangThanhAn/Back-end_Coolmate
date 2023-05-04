using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual ICollection<CartDetail>? CartDetails { get; } = new List<CartDetail>();

    public virtual User? User { get; set; } = null!;
}
