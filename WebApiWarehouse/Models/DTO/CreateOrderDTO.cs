namespace WebApiWarehouse.Models.DTO
{
    public class CreateOrderDTO
    {
        public int ClientId { get; set; }
        public int EnterpriseId { get; set; }
        public int ProductId { get; set; }
        public int QuantityO { get; set; }
    }
}
