using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Color
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? ColorText { get; set; }

    public string? ColorImg { get; set; }

    public virtual Product? Product { get; set; } = null!;
}
