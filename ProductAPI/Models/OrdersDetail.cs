using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class OrdersDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public string Size { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Order? Order { get; set; } = null!;
}
