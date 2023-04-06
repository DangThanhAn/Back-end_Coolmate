using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class CartDetail
{
    public int CartDetailId { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public string Size { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
