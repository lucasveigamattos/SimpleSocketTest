using SocketServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", polocy =>
    {
        polocy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

Chat chat = new Chat();
builder.Services.AddSingleton<IChat>(chat);

var app = builder.Build();
app.UseWebSockets();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAny");
app.Run();