using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Goolge Login 
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
   // options.LoginPath = "/account/google-login";
    //options.LoginPath = "/account/facebook-login";
}).AddGoogle(options =>
{
    options.ClientId = builder.Configuration["GoogleApi"];
    options.ClientSecret = builder.Configuration["GoogleSecretKey"];

}).AddFacebook(options =>
{
    options.ClientId = builder.Configuration["FacebookAppId"] ;
    options.ClientSecret = builder.Configuration["FacebookAppsecret"];
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
