using System;
using System.Collections.Generic;

namespace WebApiWarehouse.Models;

public partial class ActivityLog
{
    public int IdAl { get; set; }
    public int UserId { get; set; }
    public string Action { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public int Details { get; set; } 
    public virtual User User { get; set; } = null!;
}
