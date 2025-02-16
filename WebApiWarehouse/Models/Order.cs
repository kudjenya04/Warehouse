using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApiWarehouse.Models;

public partial class Order
{
    public int IdO { get; set; }
    public int ClientId { get; set; }
    public int StatusId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ProductId { get; set; }
    public int QuantityO { get; set; }

    public virtual User Client { get; set; } = null!;
    public virtual Status Status { get; set; } = null!;
    public virtual Product Product { get; set; }  // Навигационное свойство
}
