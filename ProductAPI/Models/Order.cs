using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public virtual ICollection<OrdersDetail> OrdersDetails { get; } = new List<OrdersDetail>();

    public virtual User? User { get; set; } = null!;
}
