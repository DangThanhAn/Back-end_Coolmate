using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Email { get; set; }

    public bool? Sex { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Phone { get; set; }

    public string? Role { get; set; }

    public int? Height { get; set; }

    public int? Weight { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Cart>? Carts { get; } = new List<Cart>();

    public virtual ICollection<Order>? Orders { get; } = new List<Order>();

    public virtual ICollection<Review>? Reviews { get; } = new List<Review>();
}
