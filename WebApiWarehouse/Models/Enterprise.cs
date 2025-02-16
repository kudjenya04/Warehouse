using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApiWarehouse.Models;

public partial class Enterprise
{
    public int IdE { get; set; }
    public string NameE { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Contact { get; set; } = null!;


    // Добавляем связь "один ко многим" с Product
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    [JsonIgnore]
    public ICollection<Warehouse> Warehouses { get; set; }
    public virtual ICollection<User> Users { get; set; } = new List<User>();

}
