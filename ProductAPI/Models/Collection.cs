using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Collection
{
    public int CollectionId { get; set; }

    public string? CollectionName { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
