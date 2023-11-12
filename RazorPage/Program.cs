using App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RazorPage.Models;
using RazorPage.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddOptions();
var mailsetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();


builder.Services.AddDbContext<MyBlogContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("MyBlogContext")
    )); 
builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MyBlogContext>();
;
// dang ky identity
//builder.Services.AddIdentity<AppUser, IdentityRole>()
//    .AddEntityFrameworkStores<MyBlogContext>()
//    .AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<AppUser>()
//    .AddEntityFrameworkStores<MyBlogContext>()
//    .AddDefaultTokenProviders();



builder.Services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;


});
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/login/";
    option.LogoutPath = "/logout/";
    option.AccessDeniedPath = "/khongduoctruycap.html";
});

// Đăng nhập bằng google;
builder.Services.AddAuthentication().AddGoogle(options => {
        var gconfig = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = gconfig["ClientId"];
        options.ClientSecret = gconfig["ClientSecret"];
        options.CallbackPath = "/dang-nhap-tu-google"; //cấu hình trong console.cloud google
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllowEditRole", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        //policyBuilder.RequireRole("Admin");
        //policyBuilder.RequireRole("Editor");
        policyBuilder.RequireClaim("canedit", "user");
    });
});
// Đăng ksi dịch vụ sử lỗi
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<CartService>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

// /contents/a1.jpg => Uploads/a1.jpg
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")
        ),
    RequestPath = "/contents"
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.MapRazorPages();
// để sử dụng API và lấy đến đúng route cần khai báo
app.MapControllers();
app.Run();
