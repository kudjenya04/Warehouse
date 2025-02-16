using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApiWarehouse.Models;

public partial class Warehouse
{
    public int IdW { get; set; }
    public int EnterpriseId { get; set; }
    public string NameW { get; set; } = null!;
    public string Location { get; set; } = null!;

    [JsonIgnore]
    public Enterprise Enterprise { get; set; }
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
