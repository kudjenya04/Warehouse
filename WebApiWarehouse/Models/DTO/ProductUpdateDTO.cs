namespace WebApiWarehouse.Models.DTO
{
    public class ProductUpdateDTO
    {
        public int IdP { get; set; }
        public string? NameP { get; set; }
        public string? Description { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? QuantityI { get; set; } // Количество на складе
    }
}
