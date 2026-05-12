var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// =========================
// SESSION
// =========================
builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

// =========================
// HTTP CLIENT
// =========================
builder.Services.AddHttpClient();

var app = builder.Build();

// =========================
// PIPELINE
// =========================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();