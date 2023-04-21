using System;
using System.Collections.Generic;

namespace CI_PlatForm.Entities.Models;

public partial class ContactU
{
    public long ContactUsId { get; set; }

    public long UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public virtual User User { get; set; } = null!;
}
