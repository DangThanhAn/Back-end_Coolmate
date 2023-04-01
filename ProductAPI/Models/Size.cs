using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Size
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? SizeText { get; set; }

    public int? QuantityAvalible { get; set; }

    public virtual Product Product { get; set; } = null!;
}
