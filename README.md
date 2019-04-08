# Paramore.Brighter.CommandStore.RavenDb

## How to use

During startup add the command store to the service collection with its own DocumentStore instance.

```csharp
services.AddSingleton<IAmACommandStore>(provider => new RavenDbCommandStore(new DocumentStore
{
    Urls = options.RavenDBOptions.NodeUrls,
    Database = options.RavenDBOptions.DefaultDatabase
}));
services.AddSingleton<IAmACommandStoreAsync>(provider => new RavenDbCommandStore(new DocumentStore
{
    Urls = options.RavenDBOptions.NodeUrls,
    Database = options.RavenDBOptions.DefaultDatabase
}));
```
