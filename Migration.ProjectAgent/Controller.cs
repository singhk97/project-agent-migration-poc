using Microsoft.Teams.Apps.Activities;
using Microsoft.Teams.Apps.Annotations;
using Microsoft.Teams.Apps;
using Microsoft.Teams.Api.Activities;

namespace Migration.ProjectAgent
{
    [TeamsController]
    public class Controller
    {
        [Message]
        public async Task OnMessage([Context] MessageActivity activity, [Context] IContext.Client client, [Context] Microsoft.Teams.Common.Logging.ILogger log)
        {
            await client.Typing();
            await client.Send($"hi from teams...");
        }
    }
}
