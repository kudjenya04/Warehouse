using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiWarehouse.Data;
using WebApiWarehouse.Models;
using WebApiWarehouse.Models.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace WebApiWarehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly WebApiWarwhouseDbContext _context;

        public UserController(WebApiWarwhouseDbContext context)
        {
            _context = context;
        }

        // GET: api/User — Получение всех пользователей (доступно только администратору)
        [HttpGet]
        [Authorize(Roles = "admin")]  // Ограничение доступа для администраторов
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5 — Получение пользователя по ID (доступно только администратору)
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]  // Ограничение доступа для администраторов
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            return user;
        }

        // POST: api/User — Добавление нового пользователя (доступно только администратору)
        [HttpPost]
        [Authorize(Roles = "admin")]  // Ограничение доступа для администраторов
        public async Task<ActionResult<User>> PostUser(RegisterDTO registerDto)
        {
            // Преобразование RegisterDTO в User
            var user = new User
            {
                NameU = registerDto.NameU,
                Password = registerDto.Password,
                EnterpriseId = registerDto.EnterpriseId
            };

            // Добавление пользователя в контекст
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Возвращение созданного пользователя
            return CreatedAtAction(nameof(GetUser), new { id = user.IdU }, user);
        }

    // DELETE: api/User/5 — Удалить пользователя (доступно только администратору)
    [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]  // Проверка, что роль пользователя "admin"
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.IdU == id);
        }

    // POST /api/user/admin/register — Регистрация администратора
    [HttpPost("admin/register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDTO dto)
        {
            // Проверка, что такой пользователь уже существует
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.NameU == dto.NameU);
            if (existingUser != null)
            {
                return BadRequest("Администратор с таким именем уже существует.");
            }

            // Создание нового администратора
            var newAdmin = new User
            {
                NameU = dto.NameU,
                Password = dto.Password, // Здесь можно добавить хеширование пароля
                Admin = true,
                EnterpriseId = dto.EnterpriseId
            };

            // Добавление администратора в базу данных
            _context.Users.Add(newAdmin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(RegisterAdmin), new { id = newAdmin.IdU }, newAdmin);
        }

        // POST /api/user/admin/auth — Аутентификация администратора
        [HttpPost("admin/auth")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateAdmin([FromBody] AuthenticateDTO dto)
        {
            // Ищем администратора по имени и паролю
            var admin = await _context.Users
                .FirstOrDefaultAsync(u => u.NameU == dto.NameU && u.Password == dto.Password && u.Admin);

            if (admin == null)
            {
                return Unauthorized("Неверное имя пользователя, пароль или пользователь не является администратором.");
            }

            // Создание списка заявок (claims) для аутентификации
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, admin.NameU),
            new Claim(ClaimTypes.Role, "admin") // Роль администратора
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Подписываем пользователя в систему
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(new { Message = "Успешная аутентификация администратора." });
        }

        // POST /api/user/client/auth — Аутентификация клиента
        [HttpPost("client/auth")]
        public async Task<IActionResult> AuthenticateClient([FromBody] AuthenticateDTO dto)
        {
            // Ищем клиента в базе данных
            var client = await _context.Users
                .FirstOrDefaultAsync(u => u.NameU == dto.NameU && u.Password == dto.Password && !u.Admin);

            // Если клиент не найден, возвращаем ошибку
            if (client == null)
            {
                return Unauthorized("Неверное имя пользователя, пароль или пользователь не является клиентом.");
            }

            // Определяем роль клиента, основанную на поле Admin
            var role = client.Admin ? "admin" : "regularUser"; // если Admin == true, роль будет "admin", иначе "regularUser"

            // Создаем список заявок (claims) для аутентификации
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, client.NameU),
        new Claim(ClaimTypes.Role, role) // передаем роль, которая зависит от поля Admin
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Подписываем клиента в систему
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Создаем и возвращаем UserReturnDTO, включая ID, имя пользователя и EnterpriseId
            var userDto = new UserReturnDTO
            {
                IdU = client.IdU,
                NameU = client.NameU,
                EnterpriseId = client.EnterpriseId,
                Admin = client.Admin // Добавляем роль (Admin) в DTO
            };

            // Возвращаем успешный ответ с данными пользователя
            return Ok(userDto);
        }


        // POST /api/user/client/register — Регистрация клиента
        [HttpPost("client/register")]
        public async Task<IActionResult> RegisterClient([FromBody] RegisterClientDTO dto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.NameU == dto.NameU);
            if (existingUser != null)
            {
                return Conflict("Пользователь с таким именем уже существует.");
            }
            var newClient = new User
            {
                NameU = dto.NameU,
                Password = dto.Password,
                Admin = false, // Клиент не администратор
                EnterpriseId = dto.EnterpriseId
            };
            _context.Users.Add(newClient);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Ошибка при создании клиента.");
            }
            return CreatedAtAction(nameof(RegisterClient), new { id = newClient.IdU }, new { Message = "Клиент успешно зарегистрирован." });
        }
        // POST /api/user/logout — Выход из системы
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Выход выполнен." });
        }
    }
}

