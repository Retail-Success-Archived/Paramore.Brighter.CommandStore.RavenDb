using Raven.Client.Documents;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Paramore.Brighter.CommandStore.RavenDb
{
    public class RavenDbCommandStore : IAmACommandStore, IAmACommandStoreAsync 
    {
        private IDocumentStore documentStore;

        public RavenDbCommandStore(DocumentStore store, string collectionName = "commands")
        {
            store.Conventions.FindCollectionName =
                type =>
                    type.GetGenericTypeDefinition() == typeof(Command<>) ?
                    collectionName :
                    documentStore.Conventions.GetCollectionName(type);

            documentStore = store.Initialize();
            ContinueOnCapturedContext = false;
        }

        public bool ContinueOnCapturedContext { get; set; }

        public void Add<T>(T command, int timeoutInMilliseconds) where T : class, IRequest
        {
            
            var dto = ToCommandDto(command);
            using (var session = documentStore.OpenSession())
            {
                session.Store(dto);                
                session.SaveChanges();
            }
        }

        public async Task AddAsync<T>(T command, int timeoutInMilliseconds, CancellationToken cancellationToken) where T : class, IRequest
        {
            var dto = ToCommandDto(command);
            using (var session = documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(dto, cancellationToken);
                await session.SaveChangesAsync();
            }
        }

        public T Get<T>(Guid id, int timeoutInMilliseconds) where T : class, IRequest, new()
        {
            using (var session = documentStore.OpenSession())
            {
                var command = session.Query<Command<T>>().First(x => x.CommandId == id);
                return command.CommandBody;                
            }
        }

        public async Task<T> GetAsync<T>(Guid id, int timeoutInMilliseconds, CancellationToken cancellationToken) where T : class, IRequest, new()
        {
            using (var session = documentStore.OpenAsyncSession())
            {
                var command = await session.Query<Command<T>>().FirstAsync(x => x.CommandId == id);
                return command.CommandBody;                
            }
        }

        private Command<T> ToCommandDto<T>(T command) where T : class, IRequest
        {
            return new Command<T>
            {
                CommandId = command.Id,
                CommandType = typeof(T).Name,
                CommandBody = command,
                Timestamp = DateTime.UtcNow,                
            };
        }


    }
}
