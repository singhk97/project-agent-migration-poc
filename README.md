# Teams AI POC for Project Agent Migration

## Features Showcased

1. [Setting up Teams AI app in ASP.NET application](https://github.com/singhk97/project-agent-migration-poc/blob/main/Migration.ProjectAgent/Program.cs).
2. [Teams AI app co-existing with Bot Framework adapter](https://github.com/singhk97/project-agent-migration-poc/blob/3c7c4d3a6d956cf249aa282461403b598ac5efa8/Migration.ProjectAgent/MessageController.cs#L44-L74).
3. [Proactive messaging support.](https://github.com/singhk97/project-agent-migration-poc/blob/3c7c4d3a6d956cf249aa282461403b598ac5efa8/Migration.ProjectAgent/MessageController.cs#L80-L103)
4. [Activity handling using the Teams AI Controllers.](https://github.com/singhk97/project-agent-migration-poc/blob/main/Migration.ProjectAgent/TeamsController.cs)
5. [Integrating custom bot token fetcher with Teams AI app.](https://github.com/singhk97/project-agent-migration-poc/blob/3c7c4d3a6d956cf249aa282461403b598ac5efa8/Migration.ProjectAgent/CustomAuthProvider.cs#L4-L34)

## Feature to support

| Ask                             | Details                                                                                                                                                     |
|----------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|
| HTTP Client Customization       | HTTP client to log exceptions and dependency calls for better visibility and control over parameters like connections, reuse, and disposal. Need ability to pass cv across all calls in the library. |
| Telemetry Integration           | Need for a more flexible telemetry integration that isn't tightly coupled with App Insights, allowing for custom telemetry solutions.                      |
| Request Level Metadata          | Need request-level metadata from the turn context for better telemetry and logging.                                                                         |
| Group SSO                       | Need better standard support for Group SSO on all events.                                                                                                   |
| Enable custom Cosmos DB integration | Please allow us to integrate Cosmos DB with SSO, or at minimum, adopt the R9 Cosmos DB instance.                                                              |
