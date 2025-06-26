using Microsoft.Teams.Apps.Activities;
using Microsoft.Teams.Apps.Annotations;
using Microsoft.Teams.Apps;
using Microsoft.Teams.Api.Activities;
using System.Text.Json;

namespace Migration.ProjectAgent
{
    [TeamsController]
    public class TeamsController
    {
        [Message]
        public async Task OnMessage(IContext<MessageActivity> context)
        {
            // Get the conversation reference to extract the conversation ID and service URL
            context.Log.Info(JsonSerializer.Serialize(context.Ref), new JsonSerializerOptions()
            {
                WriteIndented = true,
                IndentSize = 4
            });

            await context.Typing();
            await context.Send($"hi from teams...");
        }
    }
}
