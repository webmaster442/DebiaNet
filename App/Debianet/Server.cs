using Debianet.ServiceContract;

namespace Debianet;

public static class Server
{
    public static async Task RunServerAsync(string[] args, CancellationToken token = default)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddSingleton<IEmbededdFileProvider, Debianet.Service.EmbeddedFileProvider>();
        builder.Services.AddSingleton<ICommandRunner, Debianet.Service.CommandRunner>();

        var app = builder.Build();
        SemaphoreSlim performActionGate = new(initialCount: 1, maxCount: 1);

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/PerformAction"))
            {
                await performActionGate.WaitAsync(context.RequestAborted);

                try
                {
                    await next();
                }
                finally
                {
                    performActionGate.Release();
                }

                return;
            }

            await performActionGate.WaitAsync(context.RequestAborted);
            performActionGate.Release();

            await next();
        });

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        await app.RunAsync(token);
    }
}
