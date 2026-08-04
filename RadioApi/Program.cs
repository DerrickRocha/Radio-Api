using RadioApi.Middleware;
using RadioApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 2. Add System Components
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IRadioService, RadioService>();

var app = builder.Build();

app.UseMiddleware<RadioApiExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();