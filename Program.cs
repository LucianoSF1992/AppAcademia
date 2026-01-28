using AppAcademia.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// MVC
// ===============================
builder.Services.AddControllersWithViews();

// ===============================
// CACHE (obrigatório para Session)
// ===============================
builder.Services.AddDistributedMemoryCache();

// ===============================
// SESSION
// ===============================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ===============================
// BANCO DE DADOS
// ===============================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ===============================
// BUILD APP
// ===============================
var app = builder.Build();

// ===============================
// PIPELINE
// ===============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔐 SESSION precisa vir aqui
app.UseSession();

// ❗ NÃO usar UseAuthentication
app.UseAuthorization();

// ===============================
// ROTAS
// ===============================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();
