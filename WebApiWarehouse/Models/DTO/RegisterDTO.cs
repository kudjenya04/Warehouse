namespace WebApiWarehouse.Models.DTO
{
    public class RegisterDTO
    {
        public string NameU { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int EnterpriseId { get; set; }
    }
}
