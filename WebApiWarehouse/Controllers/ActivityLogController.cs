using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiWarehouse.Data;
using WebApiWarehouse.Models;
using WebApiWarehouse.Models.DTO;

namespace WebApiWarehouse.Controllers
{
    [Route("api/activity_log")]
    [ApiController]
    [AllowAnonymous] // Доступно всем, включая неавторизованных пользователей
    public class ActivityLogsController : ControllerBase
    {
        private readonly WebApiWarwhouseDbContext _context;
        public ActivityLogsController(WebApiWarwhouseDbContext context)
        {
            _context = context;
        }

        // GET: api/activity_log/{userId}
        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ActivityLogDTO>>> GetActivityLogsByUserId(int userId)
        {
            try
            {
                // Проверяем, существует ли пользователь с указанным userId
                var userExists = await _context.Users.AnyAsync(u => u.IdU == userId);
                if (!userExists)
                {
                    return NotFound(new { message = "Пользователь с указанным ID не найден." });
                }

                // Извлекаем записи журнала действий для указанного пользователя
                var activityLogs = await _context.ActivityLogs
                    .Where(log => log.UserId == userId)
                    .Select(log => new ActivityLogDTO
                    {
                        IdAl = log.IdAl,
                        UserId = log.UserId,
                        Action = log.Action,
                        Timestamp = log.Timestamp,
                        Details = log.Details
                    })
                    .ToListAsync();

                // Проверяем, есть ли записи для указанного пользователя
                if (!activityLogs.Any())
                {
                    return NotFound(new { message = "Для данного пользователя нет записей в журнале." });
                }

                // Возвращаем список записей в формате DTO
                return Ok(activityLogs);
            }
            catch (Exception ex)
            {
                // Логирование ошибки (можно добавить логгер)
                return StatusCode(500, new { message = "Ошибка сервера.", error = ex.Message });
            }
        }
        // POST: api/activity_log
        [HttpPost("add")]
        public async Task<ActionResult<ActivityLogDTO>> AddActivityLog(ActivityLogDTO activityLogDTO)
        {
            try
            {
                var activityLog = new ActivityLog
                {
                    UserId = activityLogDTO.UserId,
                    Action = activityLogDTO.Action,
                    Details = activityLogDTO.Details,
                    Timestamp = activityLogDTO.Timestamp
                };

                _context.ActivityLogs.Add(activityLog);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetActivityLogsByUserId), new { userId = activityLogDTO.UserId }, activityLogDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка сервера.", error = ex.Message });
            }
        }

    }
}
