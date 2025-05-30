using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NhaThuocOnline.ApiIntergration;
using NhaThuocOnline.Intergration;
using System.Security.Policy;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();

//DI
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<IPrescriptionApiClient, PrescriptionApiClient>();
builder.Services.AddTransient<ICustomerApiClient, CustomerApiClient>();
builder.Services.AddTransient<ICartApiClient, CartApiClient>();
builder.Services.AddTransient<IOrderApiClient, OrderApiClient>();
builder.Services.AddTransient<ICouponApiClient, CouponApiClient>();



//add sessions
builder.Services.AddSession(option =>
{
    option.Cookie.Name = "NtoCookie";
    option.IdleTimeout = TimeSpan.FromHours(2);

});

string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = System.TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
    };
});

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();



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
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
