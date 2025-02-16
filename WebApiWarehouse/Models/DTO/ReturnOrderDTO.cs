namespace WebApiWarehouse.Models.DTO
{
    public class ReturnOrderDTO
    {
        public int IdO { get; set; }
        public int ClientId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
        public int QuantityO { get; set; }
    }
}
