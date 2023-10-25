using DA;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPCRepo, PCRepo>();
builder.Services.AddScoped<SqlCnnFLee>(provider =>
{
    var server = "192.168.0.17";
    var database = "HSPharmacySoftBackOfficeE";
    var username = "sa";
    var password = "ClubFarmaSQLAdmin12.";

    return new SqlCnnFLee(server, database, username, password);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();