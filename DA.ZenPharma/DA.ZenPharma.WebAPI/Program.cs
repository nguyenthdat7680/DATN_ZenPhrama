using System.Security.Claims;
using System.Text;
using DA.ZenPharma.Application.Services.Implementation;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.Context;
using DA.ZenPharma.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7044") 
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials(); 
        });
});

// Add services to the container.

builder.Services.AddDbContext<ZenPharmaDbContext>(ops => ops.UseSqlServer(builder.Configuration.GetConnectionString("ZenPharmaConnection")));

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IImportInvoiceDetailService, ImportInvoiceDetailService>();
builder.Services.AddScoped<IImportInvoiceService, ImportInvoiceService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInventoryBatchService, InventoryBatchService>();

// UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();

//jwt
var jwtSetting = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSetting["Key"] ?? throw new InvalidOperationException("Jwt key is missing!"));
if (key.Length < 32)
{
    throw new InvalidOperationException("Key must be 32 character!");
}
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSetting["Issuer"],
            ValidAudience = jwtSetting["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        }
    );
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
