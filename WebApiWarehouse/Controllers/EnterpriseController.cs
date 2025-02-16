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
    public class EnterprisesController : ControllerBase
    {
        private readonly WebApiWarwhouseDbContext _context;

        public EnterprisesController(WebApiWarwhouseDbContext context)
        {
            _context = context;
        }

        // GET: api/enterprises/{id} - Информация о предприятии
        [HttpGet("{id}")]
        [AllowAnonymous] // Доступно всем, включая неавторизованных пользователей
        public async Task<ActionResult<Enterprise>> GetEnterprise(int id)
        {
            var enterprise = await _context.Enterprises
                .FirstOrDefaultAsync(e => e.IdE == id);
            if (enterprise == null)
            {
                return NotFound("Предприятие не найдено.");
            }
            return Ok(enterprise);
        }

        // GET: api/enterprises/{id}/contact - Контактная информация о предприятии
        [HttpGet("{id}/contact")]
        [AllowAnonymous] // Доступно всем, включая неавторизованных пользователей
        public async Task<ActionResult<string>> GetEnterpriseContact(int id)
        {
            var enterprise = await _context.Enterprises
                .FirstOrDefaultAsync(e => e.IdE == id);
            if (enterprise == null)
            {
                return NotFound("Предприятие не найдено.");
            }
            return Ok(enterprise.Contact);
        }
        // PATCH: api/enterprises/{id} - Частичное изменение информации о предприятии
        [HttpPatch("enterprises/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PatchEnterprise(int id, [FromBody] EnterpriseUpdateDTO updatedEnterprise)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);
            if (enterprise == null)
            {
                return NotFound("Предприятие не найдено.");
            }

            // Частичное обновление полей предприятия
            if (!string.IsNullOrEmpty(updatedEnterprise.NameE))
            {
                enterprise.NameE = updatedEnterprise.NameE;
            }
            if (!string.IsNullOrEmpty(updatedEnterprise.Address))
            {
                enterprise.Address = updatedEnterprise.Address;
            }
            if (!string.IsNullOrEmpty(updatedEnterprise.Contact))
            {
                enterprise.Contact = updatedEnterprise.Contact;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnterpriseExists(id))
                {
                    return NotFound("Предприятие не найдено после проверки.");
                }
                else
                {
                    throw;
                }
            }

            // Возвращаем обновленный объект
            var updatedEnterpriseResponse = new EnterpriseUpdateDTO
            {
                IdE = enterprise.IdE,
                NameE = enterprise.NameE,
                Address = enterprise.Address,
                Contact = enterprise.Contact
            };

            return Ok(updatedEnterpriseResponse); // Возвращаем измененную сущность
        }

        private bool EnterpriseExists(int id)
        {
            return _context.Enterprises.Any(e => e.IdE == id);
        }
    }


}
