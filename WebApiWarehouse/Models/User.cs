using System;
using System.Collections.Generic;

namespace WebApiWarehouse.Models;

public partial class User
{
    public int IdU { get; set; }
    public string NameU { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool Admin { get; set; } 
    public int EnterpriseId { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public virtual Enterprise Enterprise { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
