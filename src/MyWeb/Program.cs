using MyLib.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var applicationStartTime = DateTime.Now;
app.MapGet("/system-info", () =>
{
    var now = DateTime.Now;
    var executedTotalSeconds = (int)(now - applicationStartTime).TotalSeconds;
    return Results.Content($"""
        現在時間: {now:yyyy/MM/dd HH:mm:ss}<br/>
        啟動時間: {applicationStartTime:yyyy:MM:dd HH:mm:ss}<br/>
        當前應用程式已執行 {executedTotalSeconds.WithThousandsSeparator()} 秒
        """, "text/html; charset=utf-8");
});

app.Run();
