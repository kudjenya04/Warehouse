namespace WebApiWarehouse.Models.DTO
{
    public class UserReturnDTO
    {
        public int IdU { get; set; }
        public string NameU { get; set; } = null!;
        public int EnterpriseId { get; set; }
        public bool Admin { get; set; }
    }
}
