using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiWarehouse.Data;
using WebApiWarehouse.Models;
using WebApiWarehouse.Models.DTO;

namespace WebApiWarehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly WebApiWarwhouseDbContext _context;

        public OrdersController(WebApiWarwhouseDbContext context)
        {
            _context = context;
        }

        // GET /api/orders?clientId={clientId} — Получение списка заказов для клиента
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ShowOrderDTO>>> GetOrders([FromQuery] int clientId)
        {
            if (clientId <= 0)
            {
                return BadRequest($"Некорректный ID клиента: {clientId}. Убедитесь, что ID передан и больше нуля.");
            }

            var orders = await _context.Orders
                .Where(o => o.ClientId == clientId) // Фильтруем заказы по ID клиента
                .Join(
                    _context.Products,                // Соединяем с таблицей продуктов
                    order => order.ProductId,         // Условие соединения по ProductId
                    product => product.IdP,
                    (order, product) => new ShowOrderDTO
                    {
                        IdO = order.IdO,
                        ClientId = order.ClientId,
                        StatusId = order.StatusId,
                        CreatedAt = order.CreatedAt,
                        ProductId = order.ProductId,
                        QuantityO = order.QuantityO,
                        NameP = product.NameP,   // Добавляем название продукта
                        UnitPrice = product.UnitPrice // Добавляем цену за единицу
                    })
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound($"Заказы для клиента с ID {clientId} не найдены.");
            }

            return Ok(orders);
        }

        // GET /api/orders?statusId={statusId} — Получение списка заказов с фильтрацией по статусу
        [HttpGet("status")]  // Указываем маршрут "status"
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OrderforAdminDTO>>> GetOrdersByStatus([FromQuery] int statusId)
        {
            if (statusId < 0)
            {
                return BadRequest($"Некорректный статус: {statusId}. Статус должен быть больше или равен нулю.");
            }

            var ordersQuery = _context.Orders.AsQueryable(); // Получаем все заказы

            if (statusId > 0)
            {
                ordersQuery = ordersQuery.Where(o => o.StatusId == statusId); // Фильтруем заказы по статусу
            }

            var orders = await ordersQuery
                .Join(
                    _context.Products,                // Соединяем с таблицей продуктов
                    order => order.ProductId,         // Условие соединения по ProductId
                    product => product.IdP,
                    (order, product) => new OrderforAdminDTO
                    {
                        IdO = order.IdO,
                        ClientId = order.ClientId,
                        StatusId = order.StatusId,
                        CreatedAt = order.CreatedAt,
                        ProductId = order.ProductId,
                        QuantityO = order.QuantityO,
                        NameP = product.NameP,   // Добавляем название продукта
                        UnitPrice = product.UnitPrice // Добавляем цену за единицу
                    })
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound($"Заказы с статусом {statusId} не найдены.");
            }

            return Ok(orders);
        }

        // GET /api/orders/{id} — Получение информации о конкретном заказе
        [HttpGet("{id}")]
        [AllowAnonymous] //  для всех
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.IdO == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }

        // POST /api/orders — Создание нового заказа
        [HttpPost]
        [AllowAnonymous] // Только для клиентов
        public async Task<ActionResult<ReturnOrderDTO>> PostOrder([FromBody] CreateOrderDTO dto)
        {
            // Проверяем, существует ли статус "В ожидании"
            var pendingStatus = await _context.Statuses.FindAsync(1); // ID статуса "В ожидании"
            if (pendingStatus == null)
            {
                return BadRequest("Статус 'В ожидании' не найден в системе.");
            }

            // Проверяем, существует ли продукт
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
            {
                return NotFound("Продукт с указанным ID не найден.");
            }

            // Проверяем, существует ли клиент
            var client = await _context.Users.FindAsync(dto.ClientId);
            if (client == null)
            {
                return NotFound("Клиент с указанным ID не найден.");
            }

            // Создаём новый заказ
            var newOrder = new Order
            {
                ClientId = dto.ClientId,
                ProductId = dto.ProductId,
                QuantityO = dto.QuantityO,
                StatusId = pendingStatus.IdStatus, // Присваиваем статус "В ожидании"
                CreatedAt = DateTime.UtcNow
            };
            // Добавляем заказ в базу данных
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            // Создаём DTO для возвращаемого объекта
            var returnOrderDTO = new ReturnOrderDTO
            {
                IdO = newOrder.IdO,
                ClientId = newOrder.ClientId,
                StatusId = newOrder.StatusId,
                CreatedAt = newOrder.CreatedAt,
                ProductId = newOrder.ProductId,
                QuantityO = newOrder.QuantityO
            };

            // Возвращаем DTO с деталями созданного заказа
            return CreatedAtAction(nameof(GetOrder), new { id = newOrder.IdO }, returnOrderDTO);
        }

        // PUT /api/orders/{id} — Обновление статуса заказа(а надо ли эта точка? возможно здесть надо сделать для клиента изменить свой заказ)
        [HttpPut("{id}")]
        [Authorize(Policy = "admin")] // Только для администраторов
        public async Task<IActionResult> PutOrder(int id, [FromBody] UpdateOrderStatusDTO dto)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.StatusId = dto.StatusId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(o => o.IdO == id))
                {
                    return NotFound("Order not found.");
                }
                throw;
            }

            return NoContent();
        }

        // DELETE /api/orders/{id} — Удаление заказа
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")] // Только для администраторов
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH /api/orders/{id}/status — Обновление статуса заказа
        [HttpPatch("{id}/status")]
        [AllowAnonymous]  // Только для администраторов
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusDTO dto)
        {
            // Проверяем существование заказа
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound($"Заказ с ID {id} не найден.");
            }

            // Проверяем, существует ли переданный статус
            var status = await _context.Statuses.FindAsync(dto.IdStatus);
            if (status == null)
            {
                return BadRequest($"Статус с ID {dto.IdStatus} не найден.");
            }

            // Обновляем статус заказа
            order.StatusId = dto.IdStatus;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Произошла ошибка при обновлении статуса заказа.");
            }

            return NoContent(); // Статус 204 - успешное обновление
        }

    }
}
