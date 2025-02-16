namespace WebApiWarehouse.Models.DTO
{
    public class CreateWarehouseDTO
    {
        public string NameW { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int EnterpriseId { get; set; }
    }
}
