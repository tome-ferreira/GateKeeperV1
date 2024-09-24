using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services.ServicoEmail;
using GateKeeperV1.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add database.
var connectionString = builder.Configuration.GetConnectionString("GateKeeperV1_DB") ?? throw new InvalidOperationException("Connection string 'GateKeeperV1_DB' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<IdentityOptions>(options =>
{
    // Configure password requirements
    options.Password.RequireDigit = true;           // Requires at least one numeric character
    options.Password.RequireLowercase = true;       // Requires at least one lowercase character
    options.Password.RequireUppercase = true;       // Requires at least one uppercase character
    options.Password.RequireNonAlphanumeric = true; // Requires at least one non-alphanumeric character
    options.Password.RequiredLength = 8;            // Minimum password length
    options.Password.RequiredUniqueChars = 6;       // Minimum unique characters within the password

    // You can further customize other Identity options here.
    //options.SignIn.RequireConfirmedEmail = false;
});




//Add identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


//Add email service
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();




builder.Services.ConfigureApplicationCookie(options =>
{
    // Set the login path to your custom Account controller's login action
    options.LoginPath = "/Account/Login";

    // You can also configure other paths here, e.g., AccessDeniedPath, LogoutPath, etc.
    //options.AccessDeniedPath = "/Account/AccessDenied";
});

//Costum functions 
builder.Services.AddScoped<IFunctions, Functions>();

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
