using Microsoft.Extensions.FileProviders;
using shopsport.Extentions;
using shopsport.Middleware;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder =>
		{
			builder.WithOrigins("http://192.168.243.125:3000", "http://192.168.9.104:7285")
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		});
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigDIBusinessService();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureMigration();
builder.Services.ConfigureAuth(builder.Configuration);
var app = builder.Build();
var env = app.Environment;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Images")),
	RequestPath = "/Images"
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
