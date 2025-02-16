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
    public class ProductsController : ControllerBase
    {
        private readonly WebApiWarwhouseDbContext _context;

        public ProductsController(WebApiWarwhouseDbContext context)
        {
            _context = context;
        }

        // GET: api/products — Список продуктов для клиента.
        [HttpGet]
        [AllowAnonymous] // Доступно всем, включая неавторизованных пользователей
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] int? enterpriseId)
        {
            if (enterpriseId == null)
            {
                return BadRequest("Enterprise ID is required.");
            }

            var products = await _context.Products
                .Where(p => p.EnterpriseId == enterpriseId) // Фильтрация по enterpriseId
                .Include(p => p.Inventories) // Подключаем связи с Inventories
                .Include(p => p.Order) // Подключаем связи с Order
                .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound($"No products found for enterprise ID {enterpriseId}.");
            }

            return Ok(products);
        }

        // PATCH: api/products/{id} - Частичное изменение информации о продукте
        [HttpPatch("products/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] ProductUpdateDTO updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Продукт не найден.");
            }
            // Частичное обновление полей продукта
            if (!string.IsNullOrEmpty(updatedProduct.NameP))
            {
                product.NameP = updatedProduct.NameP;
            }
            if (!string.IsNullOrEmpty(updatedProduct.Description))
            {
                product.Description = updatedProduct.Description;
            }
            if (updatedProduct.UnitPrice.HasValue)
            {
                product.UnitPrice = updatedProduct.UnitPrice.Value;
            }
            // Обновление количества в инвентаре
            if (updatedProduct.QuantityI.HasValue)
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ProductId == id);

                if (inventory != null)
                {
                    inventory.QuantityI = updatedProduct.QuantityI.Value;
                }
                else
                {
                    return BadRequest("Инвентарь для продукта не найден.");
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound("Продукт не найден после проверки.");
                }
                else
                {
                    throw;
                }
            }
            // Формируем ответ с обновленными данными
            var updatedProductResponse = new ProductUpdateDTO
            {
                IdP = product.IdP,
                NameP = product.NameP,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                QuantityI = updatedProduct.QuantityI
            };

            return Ok(updatedProductResponse); // Возвращаем измененную сущность
        }

        // DELETE: api/products/{id} — Удаление продукта (доступно для администратора).
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")] // Только для администраторов
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.IdP == id);
        }
    }
}