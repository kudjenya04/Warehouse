namespace WebApiWarehouse.Models.DTO
{
    public class ActivityLogDTO
    {
            public int IdAl { get; set; } // Идентификатор действия
             public int UserId { get; set; }
             public string Action { get; set; } = string.Empty; // Действие, выполненное пользователем
            public DateTime Timestamp { get; set; } // Время выполнения действия
            public int Details { get; set; }  // Дополнительная информация
    }
}
