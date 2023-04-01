using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string? ProductTypeName { get; set; }

    public string? Describe { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();
}
