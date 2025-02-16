﻿namespace WebApiWarehouse.Models.DTO
{
    public class OrderforAdminDTO
    {
        public int IdO { get; set; }
        public int ClientId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
        public int QuantityO { get; set; }
        public string NameP { get; set; } // Название продукта
        public decimal UnitPrice { get; set; } // Цена за единицу
    }
}
