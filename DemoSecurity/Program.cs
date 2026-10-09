var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddAuthentication().AddJwtBearer(o => o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//{
//    ValidateLifetime = true,
//    ValidateIssuerSigningKey = true,
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(b =>
{
    //b.AddDefaultPolicy(o => o.AllowAnyHeader().WithOrigins("https://mojovelo.be").WithMethods("GET", "POST").AllowCredentials());
    b.AddDefaultPolicy(o => o.AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
