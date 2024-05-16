 using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text; 
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

 

// Configure CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowOrigin", builder => {
        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

// Configure FluentEmail
builder.Services.AddFluentEmail("nexawo.noreply@gmail.com")
    .AddSmtpSender(new System.Net.Mail.SmtpClient("smtp.gmail.com") {
        EnableSsl = true,
        DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
        Port = 587,
        UseDefaultCredentials = false,
        Credentials = new System.Net.NetworkCredential("nexawo.noreply@gmail.com", "vxsvmqiqxgkdblox")
    });

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PracticeCrud", Version = "v1" });
});

// Configure database context
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

// Configure authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x => {
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "localhost",
        ValidAudience = "localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtConfig:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PracticeCrud v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
