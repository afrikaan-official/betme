var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("nosyapi",
    client =>
    {
        client.BaseAddress = new Uri("https://www.nosyapi.com/apiv2/service/");
        client.DefaultRequestHeaders.Add("Authorization","Bearer 2A8Xl1YcnEY0hYAHVnz8TMWkXCVyM7g6fmK0Q981BKR6P8oG8PGP2w3ePKOK");
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();