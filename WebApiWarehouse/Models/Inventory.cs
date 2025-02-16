using System;
using System.Collections.Generic;

namespace WebApiWarehouse.Models;

public partial class Inventory
{
    public int IdI { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public int QuantityI { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual Warehouse Warehouse { get; set; } = null!;
}
