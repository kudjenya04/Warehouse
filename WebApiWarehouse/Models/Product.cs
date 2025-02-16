using System;
using System.Collections.Generic;

namespace WebApiWarehouse.Models;

public partial class Product
{
    public int IdP { get; set; }
    public string NameP { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int EnterpriseId { get; set; }
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    // Связь с Enterprise, это внешний ключ
    public virtual Enterprise Enterprise { get; set; } = null!;

}
