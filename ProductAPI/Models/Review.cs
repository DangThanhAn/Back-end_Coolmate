using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Review
{
    public int Id { get; set; }

    public int? ProductTypeId { get; set; }

    public int? UserId { get; set; }

    public string? Describe { get; set; }

    public string? Image { get; set; }

    public int? Star { get; set; }

    public DateTime? DateReview { get; set; }

    public virtual ProductType? ProductType { get; set; }

    public virtual User? User { get; set; }
}
