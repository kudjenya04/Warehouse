using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiWarehouse.Data;

var builder = WebApplication.CreateBuilder(args);


// ����������� � ���� ������
builder.Services.AddDbContext<WebApiWarwhouseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ������������ ������������
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // ������� ����������� ������
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// ���������� �������������� ����� cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/user/client/auth";
        options.LogoutPath = "/api/user/logout";
        options.AccessDeniedPath = "/api/user/access-denied";
        options.LoginPath = "/api/user/admin/auth"; // ���� ��� �������������� ������
        options.SlidingExpiration = true;
    });

// ������������� �����
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin"));

    options.AddPolicy("regularUser", policy =>
        policy.RequireClaim(ClaimTypes.Role, "regularUser"));
});

// ���������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// ���������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ������������� ���� ������
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WebApiWarwhouseDbContext>();
    dbContext.Database.Migrate();
}

// ��������� ������������� � ��������� ������
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

// ��������� �������� CORS
app.UseCors("AllowLocalhost");

// ����������� �������� ��� API
app.UseHttpsRedirection();
app.UseRouting();

// ���������� �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

// ��������� API ������������
app.MapControllers();

// ������ ����������
app.Run();
