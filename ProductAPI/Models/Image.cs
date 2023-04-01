using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Image
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? ImgUrl { get; set; }

}
