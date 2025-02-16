namespace WebApiWarehouse.Models.DTO
{
    public class UpdateInventoryDTO
    {
            public int OrderId { get; set; }      // ID заказа
            public int ProductId { get; set; }   // ID продукта
            public int Quantity { get; set; }    // Количество товара для вычитания
    }
}
