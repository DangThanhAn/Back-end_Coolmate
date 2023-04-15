using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual Product? Product { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
