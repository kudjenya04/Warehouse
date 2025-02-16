namespace WebApiWarehouse.Models.DTO
{
    public class EnterpriseUpdateDTO
    {
        public int IdE { get; set; }

        public string NameE { get; set; }  // Название предприятия (необязательное поле)

        public string Address { get; set; }  // Адрес предприятия (необязательное поле)

        public string Contact { get; set; }  // Контактная информация (необязательное поле)
    }
}
