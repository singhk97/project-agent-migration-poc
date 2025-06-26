using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Teams.Api.Activities;
using Microsoft.Teams.Api.Auth;
using Microsoft.Teams.Apps;
using Microsoft.Teams.Apps.Extensions;
using Microsoft.Teams.Plugins.AspNetCore;
using System.Text.Json;

namespace Migration.ProjectAgent
{
    /// <summary>
    /// Controller for handling incoming messages and sending proactive messages.
    /// </summary>
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly IBot _bot;
        
        private readonly ILogger<MessageController> _logger;
        
        private readonly AspNetCorePlugin _plugin;
        private readonly IHostApplicationLifetime _lifetime;
        
        private readonly App _app;

        public MessageController(ILogger<MessageController> logger, App app, AspNetCorePlugin plugin, IHostApplicationLifetime lifetime, IBotFrameworkHttpAdapter adapter, IBot bot)
        {
            _logger = logger;
            _app = app;
            _plugin = plugin;
            _lifetime = lifetime;
            _adapter = adapter;
            _bot = bot;
        }

        /// <summary>
        /// POST /api/messages
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/messages")]
        public async Task<IResult> OnMessage()
        {
            // [PA Migration] This shows how Teams AI app can be used to handle reactive messaging scenarios alongside the BotBuilder adapter.
            // [PA Migration] This allows for a phased migration from BotBuilder to Teams AI, where the BotBuilder adapter can still be used for handling incoming messages.
            HttpContext.Request.EnableBuffering();
            var body = await new StreamReader(Request.Body).ReadToEndAsync();
            Activity? activity = JsonSerializer.Deserialize<Activity>(body);
            HttpContext.Request.Body.Position = 0;

            if (activity == null)
            {
                return Results.BadRequest("Missing activity");
            }

            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await _adapter.ProcessAsync(HttpContext.Request, HttpContext.Response, _bot);

            if (Response.HasStarted)
            {
                return Results.Empty;
            }

            // Fallback logic
            var authHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault() ?? throw new UnauthorizedAccessException();
            var token = new JsonWebToken(authHeader.Replace("Bearer ", ""));
            var context = HttpContext.RequestServices.GetRequiredService<TeamsContext>();
            context.Token = token;
            var res = await _plugin.Do(token, activity, _lifetime.ApplicationStopping);
            return Results.Json(res.Body, statusCode: (int)res.Status);
        }

        /// <summary>
        /// POST /welcome/callback
        /// </summary>
        /// <returns></returns>
        [HttpPost("/welcome/callback")]
        public async Task<IActionResult> SendWelcomeMessage()
        {
            // [PA Migration] This endpoint is used to send a proactive welcome message to a specific conversation.
            _logger.LogInformation("/welcome/callback hit");
            var conversationId = "a:1WxuE0NTjQjGgI3InNWAEWpqNo-r2Sj2_3GthNnG_APSquhea5MQxByt6pEXF5hwDHjnsh4aJdgJoFaeGSKCrToMN3tLv6plj4bSSH9DKtEAmVL28tqlI0A2Lu4VZHlNy";
            var serviceUrl = "https://smba.trafficmanager.net/amer/72f988bf-86f1-41af-91ab-2d7cd011db47/";

            try
            {
                await _app.Send(conversationId, "Proactive Welcome Message", serviceUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send welcome message to conversation ID: {ConversationId}", conversationId);
                return StatusCode(500, "Failed to send welcome message");
            }


            _logger.LogInformation("Welcome message sent to conversation ID: {ConversationId}", conversationId);
            
            return Ok();
        }
    }
}
