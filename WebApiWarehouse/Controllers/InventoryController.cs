using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiWarehouse.Data;
using WebApiWarehouse.Models;
using WebApiWarehouse.Models.DTO;

namespace WebApiWarehouse.Controllers
{

    [Route("api/[controller]")]
    [ApiController] // Все методы в этом контроллере доступны только администраторам
    public class InventoryController : ControllerBase
    {
        private readonly WebApiWarwhouseDbContext _context;

        public InventoryController(WebApiWarwhouseDbContext context)
        {
            _context = context;
        }
        // GET /api/inventory - Запасы на всех складах.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventory()
        {
            var inventory = await _context.Inventories
                                          .Include(i => i.Product)
                                          .Include(i => i.Warehouse)
                                          .ToListAsync();
            return Ok(inventory);
        }

        // GET /api/inventory/{id} — Запасы на складе.
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventoryByWarehouseId(int id)
        {
            var inventory = await _context.Inventories
                                          .Where(i => i.WarehouseId == id)
                                          .Include(i => i.Product)
                                          .Include(i => i.Warehouse)
                                          .ToListAsync();

            if (inventory == null || !inventory.Any())
            {
                return NotFound($"No inventory found for warehouse with ID {id}");
            }

            return Ok(inventory);
        }

        // POST /api/inventory — Пополнение запасов
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory(AddQuantityIDTO inventoryDto)
        {
            // Проверка на существование продукта и склада
            var product = await _context.Products.FindAsync(inventoryDto.ProductId);
            var warehouse = await _context.Warehouses.FindAsync(inventoryDto.WarehouseId);
            if (product == null)
            {
                return BadRequest("Product not found.");
            }
            if (warehouse == null)
            {
                return BadRequest("Warehouse not found.");
            }
            // Создание новой записи для Inventory
            var inventory = new Inventory
            {
                ProductId = inventoryDto.ProductId,
                WarehouseId = inventoryDto.WarehouseId,
                QuantityI = inventoryDto.QuantityI
            };
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventoryById), new { id = inventory.IdI }, inventory);
        }

        // PUT /api/inventory/{id} — Обновление информации о запасах
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory(int id, [FromBody] EditInventaryDTO dto)
        {
            // Проверяем, существует ли запись инвентаря с указанным ID
            var existingInventory = await _context.Inventories.FindAsync(id);
            if (existingInventory == null)
            {
                return NotFound("Инвентарь не найден.");
            }
            // Проверка существования продукта и склада
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
            {
                return BadRequest("Продукт не найден.");
            }
            var warehouse = await _context.Warehouses.FindAsync(dto.WarehouseId);
            if (warehouse == null)
            {
                return BadRequest("Склад не найден.");
            }
            // Обновляем данные инвентаря
            existingInventory.QuantityI = dto.QuantityI;
            existingInventory.ProductId = dto.ProductId;
            existingInventory.WarehouseId = dto.WarehouseId;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Произошла ошибка при обновлении инвентаря: {ex.Message}");
            }
            return NoContent(); // Возвращаем статус 204 для успешного обновления
        }

        // DELETE /api/inventory/{id} — Удаление записи о запасах
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            // Ищем запись инвентаря для удаления
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound("Инвентарь не найден.");
            }

            // Удаляем запись инвентаря
            _context.Inventories.Remove(inventory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Произошла ошибка при удалении инвентаря.");
            }

            return NoContent();  // Возвращаем статус 204 для успешного удаления
        }

        // GET /api/inventory/{id} — Получение инвентаря по ID (помощник для POST ответа)
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventoryById(int id)
        {
            var inventory = await _context.Inventories
                                          .Include(i => i.Product)
                                          .Include(i => i.Warehouse)
                                          .FirstOrDefaultAsync(i => i.IdI == id);

            if (inventory == null)
            {
                return NotFound();
            }

            return inventory;
        }


        // PATCH: api/inventory — изменение количества на сткладе
        [HttpPatch]
        [AllowAnonymous] // Доступно всем, включая неавторизованных пользователей
        public async Task<IActionResult> UpdateInventory([FromBody] UpdateInventoryDTO dto)
        {
            // Проверяем наличие заказа
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.IdO == dto.OrderId);
            if (order == null)
                return NotFound(new { message = "Заказ не найден." });

            // Проверяем наличие товара на складе
            var inventoryItem = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == dto.ProductId);
            if (inventoryItem == null)
                return NotFound(new { message = "Товар на складе не найден." });

            // Проверяем, достаточно ли товара на складе
            if (inventoryItem.QuantityI < dto.Quantity)
                return BadRequest(new { message = "Недостаточно товара на складе для выполнения заказа." });

            // Обновляем количество на складе
            inventoryItem.QuantityI -= dto.Quantity;

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Количество на складе успешно обновлено.",
                updatedQuantity = inventoryItem.QuantityI
            });
        }
    }
}