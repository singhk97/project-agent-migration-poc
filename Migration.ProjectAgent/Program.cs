using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Teams.Apps.Extensions;
using Microsoft.Teams.Plugins.AspNetCore.DevTools.Extensions;
using Microsoft.Teams.Plugins.AspNetCore.Extensions;

namespace Migration.ProjectAgent;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddTransient<Controller>();
        builder
            .AddTeams()
            .AddTeamsDevTools()
            .AddBotBuilder<Bot, BotBuilderAdapter, ConfigurationBotFrameworkAuthentication>();

        var app = builder.Build();

        app.UseTeams();
        app.Run();
    }
}