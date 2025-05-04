var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// builder.Services.AddHttpClient("nosyapi",
//     client =>
//     {
//         client.BaseAddress = new Uri("https://www.nosyapi.com/apiv2/service/");
//         client.DefaultRequestHeaders.Add("Authorization","Bearer OtMp3WiiotJCjHhUAdi2ma9kMZicohMdND9gDtcQ80W2iIIvtX6v29Hrabe3");
//     });

builder.Services.AddHttpClient("bilyoner",
    client =>
    {
        //client.BaseAddress = new Uri("https://www.nosyapi.com/apiv2/service/");
        //client.DefaultRequestHeaders.Add("Authorization","Bearer OtMp3WiiotJCjHhUAdi2ma9kMZicohMdND9gDtcQ80W2iIIvtX6v29Hrabe3");
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