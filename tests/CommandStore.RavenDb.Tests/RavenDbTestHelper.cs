using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace CommandStore.RavenDb.Tests
{

    public class RavenDbTestHelper
    {
        private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);
        private static Lazy<RavenDbSettings> ravenSettings = new Lazy<RavenDbSettings>(CreateSettings);
        public static IDocumentStore Store => store.Value;
        public static RavenDbSettings RavenDbSettings => ravenSettings.Value;

        private static RavenDbSettings CreateSettings() 
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json")
                .AddUserSecrets<TestSettings>();
            var config = builder.Build();

            var settings = new RavenDbSettings();
            config.GetSection("RavenDbSettings").Bind(settings);
            return settings;
        }
        private static IDocumentStore CreateStore()
        {
            IDocumentStore store = new DocumentStore()
            {
                Urls = RavenDbSettings.NodeUrls,
                Database = RavenDbSettings.TestDatabaseName
            }.Initialize();

            return store;
        }

        public RavenDbTestHelper()
        {

        }

        public void CreateFreshTestDatabase()
        {
            if(string.IsNullOrWhiteSpace(RavenDbSettings.TestDatabaseName))
                throw new ArgumentNullException(nameof(RavenDbSettings.TestDatabaseName));

            try
            {
                Store.Maintenance.ForDatabase(RavenDbSettings.TestDatabaseName).Send(new GetStatisticsOperation());
                //it didn't throw so it exists and we can delete it
                var deleteOperation = new DeleteDatabasesOperation(RavenDbSettings.TestDatabaseName, true);
                Store.Maintenance.Server.Send(deleteOperation);
            }
            catch(DatabaseDoesNotExistException)
            {
                
            }

            try
            {
                var createOperation = new CreateDatabaseOperation(new DatabaseRecord(RavenDbSettings.TestDatabaseName));
                Store.Maintenance.Server.Send(createOperation);
            }
            catch (ConcurrencyException)
            {
                // Something created the database already
            }
            
        }

        public void DeleteTestDatabase()
        {
            var deleteOperation = new DeleteDatabasesOperation(RavenDbSettings.TestDatabaseName, true);
            Store.Maintenance.Server.Send(deleteOperation);
        }
    }     

}
