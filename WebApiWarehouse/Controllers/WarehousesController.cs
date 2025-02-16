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
    [Authorize(Policy = "admin")] // Только для администраторов
    public class WarehousesController : ControllerBase
    {
        private readonly WebApiWarwhouseDbContext _context;

        public WarehousesController(WebApiWarwhouseDbContext context)
        {
            _context = context;
        }

        // GET: api/warehouses — Список складов.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
        {
            var warehouses = await _context.Warehouses
                .Include(w => w.Enterprise) // Подключаем связь с Enterprise
                .ToListAsync();

            if (warehouses == null || warehouses.Count == 0)
            {
                return NotFound("No warehouses found.");
            }

            return Ok(warehouses);
        }

        // GET: api/warehouses/{id} — Информация о складе.
        [HttpGet("{id}")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.Enterprise) // Подключаем связь с Enterprise
                .FirstOrDefaultAsync(w => w.IdW == id);

            if (warehouse == null)
            {
                return NotFound("Warehouse not found.");
            }

            return Ok(warehouse);
        }

        // POST: api/warehouses — Создание склада
        [HttpPost]
        public async Task<ActionResult<Warehouse>> PostWarehouse(CreateWarehouseDTO createWarehouseDto)
        {
            if (createWarehouseDto == null)
            {
                return BadRequest("Invalid warehouse data.");
            }

            // Проверяем существование предприятия с заданным EnterpriseId
            var enterpriseExists = await _context.Enterprises.AnyAsync(e => e.IdE == createWarehouseDto.EnterpriseId);
            if (!enterpriseExists)
            {
                return BadRequest($"Enterprise with ID {createWarehouseDto.EnterpriseId} does not exist.");
            }

            // Создаем новый склад
            var warehouse = new Warehouse
            {
                NameW = createWarehouseDto.NameW,
                Location = createWarehouseDto.Location,
                EnterpriseId = createWarehouseDto.EnterpriseId
            };

            // Добавляем склад в контекст
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();

            // Возвращаем статус Created с информацией о созданном складе
            return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.IdW }, warehouse);
        }

        // PUT: api/warehouses/{id} — Обновление склада.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse(int id, EditWarehouseDTO editWarehouseDTO)
        {
            // Проверяем, существует ли склад с заданным ID
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound("Warehouse not found.");
            }
            // Обновляем поля склада из DTO
            warehouse.NameW = editWarehouseDTO.NameW;
            warehouse.Location = editWarehouseDTO.Location;

            // Помечаем запись как измененную
            _context.Entry(warehouse).State = EntityState.Modified;
            try
            {
                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Проверяем, существует ли склад в базе данных
                if (!WarehouseExists(id))
                {
                    return NotFound("Warehouse not found.");
                }
                else
                {
                    throw;
                }
            }
            // Возвращаем статус 204 (No Content) при успешном обновлении
            return NoContent();
        }

        // DELETE: api/warehouses/{id} — Удаление склада.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound("Warehouse not found.");
            }

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.IdW == id);
        }
    }
}
