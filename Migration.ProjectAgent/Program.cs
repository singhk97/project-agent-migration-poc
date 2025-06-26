using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Teams.Api.Auth;
using Microsoft.Teams.Apps;
using Microsoft.Teams.Apps.Extensions;
using Microsoft.Teams.Extensions.Logging;
using Microsoft.Teams.Plugins.AspNetCore;
using Microsoft.Teams.Plugins.AspNetCore.DevTools.Extensions;
using Microsoft.Teams.Plugins.AspNetCore.Extensions;

namespace Migration.ProjectAgent;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ASP.NET Controllers
        // TODO: Meet with Alex to figure out why BotBuilder & AspNetCorePlugin controllers are being auto-added.
        builder.Services.AddControllers().ConfigureApplicationPartManager(apm =>
        {
            // Keep nothing except *this* assembly
            apm.ApplicationParts.Clear();
            apm.ApplicationParts
               .Add(new AssemblyPart(typeof(MessageController).Assembly));
        });

        // Teams AI Controller (essentially activity handling logic)
        builder.Services.AddTransient<TeamsController>();

        // Setup BotAuthProvider
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddSingleton(sp =>
        {
            var settings = builder.Configuration.GetTeams();
            return new ClientCredentials(settings.ClientId!, settings.ClientSecret!) 
            {
                TenantId = settings.TenantId,
            };
        });
        builder.Services.AddSingleton<Microsoft.Teams.Common.Http.IHttpCredentials, CustomAuthProvider>();

        // Setup configuration and Teams AI loggin
        builder.Services.AddSingleton(builder.Configuration.GetTeams());
        builder.Services.AddSingleton(builder.Configuration.GetTeamsLogging());
        builder.Logging.AddTeams();

        // Setup services needed to run Teams AI app.
        builder.Services.AddTeams((sp) => 
        {
            var credentials = sp.GetRequiredService<Microsoft.Teams.Common.Http.IHttpCredentials>();
            var options = new AppOptions()
            {
                Credentials = credentials
            };

            var app = new App(options);

            return app;
        });

        // Integrating Bot Framework Setup with Teams AI setup.
        builder.Services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();
        builder.Services.AddSingleton<IBotFrameworkHttpAdapter, BotBuilderAdapter>();
        builder.Services.AddTransient<IBot, Bot>();

        // AspNetCorePlugin is invoked from the controller.
        // Through the AspNetCorePlugin.Do() method it invokes the app activity processing logic.
        builder
            .AddTeamsPlugin<AspNetCorePlugin>()
            .AddTeamsDevTools();

        var app = builder.Build();

        // Registers the TeamsController and initializes plugins.
        app.UseTeams(routing: false);

        app.UseRouting();
        app.UseEndpoints(endpoint => endpoint.MapControllers());

        app.Run();
    }
}