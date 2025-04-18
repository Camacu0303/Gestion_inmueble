using System.Net.Http.Headers;
using WEB.Util;
using WebApplication125.Pages;

var builder = WebApplication.CreateBuilder(args);
var apiUrl = builder.Configuration.GetValue<string>("ApiUrl");
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7094/api/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
});
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
