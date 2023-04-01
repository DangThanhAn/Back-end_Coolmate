using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Product
{
    public int Id { get; set; }

    public int? ProductTypeId { get; set; }

    public int? CollectionId { get; set; }

    public int? CategoryId { get; set; }

    public string? ProductName { get; set; }

    public int? Quantity { get; set; }

    public int? QuantityAvailable { get; set; }

    public decimal? Price { get; set; }

    public string? Sale { get; set; }

    //public virtual Category? Category { get; set; }

    //public virtual Collection? Collection { get; set; }

    public virtual ICollection<Color> Colors { get; } = new List<Color>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    //public virtual ProductType? ProductType { get; set; }

    public virtual ICollection<Size> Sizes { get; } = new List<Size>();
}
