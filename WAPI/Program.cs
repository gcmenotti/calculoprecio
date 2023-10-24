using DA;
using MODEL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<SqlCnnFLee>(provider =>
{
   string server = "192.168.0.17";
   string database = "HSPharmacySoftBackOfficeE";
   string username = "sa";
   string password = "ClubFarmaSQLAdmin12.";

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