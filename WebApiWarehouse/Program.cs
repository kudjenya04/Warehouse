using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiWarehouse.Data;

var builder = WebApplication.CreateBuilder(args);


// Подключение к базе данных
builder.Services.AddDbContext<WebApiWarwhouseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Конфигурация контроллеров
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Убираем циклические ссылки
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Добавление аутентификации через cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/user/client/auth";
        options.LogoutPath = "/api/user/logout";
        options.AccessDeniedPath = "/api/user/access-denied";
        options.LoginPath = "/api/user/admin/auth"; // Путь для аутентификации админа
        options.SlidingExpiration = true;
    });

// Разграничение ролей
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin"));

    options.AddPolicy("regularUser", policy =>
        policy.RequireClaim(ClaimTypes.Role, "regularUser"));
});

// Добавление CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// Добавление Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WebApiWarwhouseDbContext>();
    dbContext.Database.Migrate();
}

// Настройка маршрутизации и обработки ошибок
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Warehouse API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");//jfegfeg
    app.UseHsts();
}

// Применяем политику CORS
app.UseCors("AllowLocalhost");

// Стандартные маршруты для API
app.UseHttpsRedirection();
app.UseRouting();

// Обработчик аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Настройка API контроллеров
app.MapControllers();

// Запуск приложения
app.Run();
