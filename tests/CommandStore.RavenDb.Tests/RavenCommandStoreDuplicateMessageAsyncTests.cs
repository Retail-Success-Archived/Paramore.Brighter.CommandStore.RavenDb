using System;
using Paramore.Brighter;
using Xunit;
using Paramore.Brighter.CommandStore.RavenDb;
using System.Threading.Tasks;

namespace CommandStore.RavenDb.Tests
{
    [Trait("Category", "RavenDb")]
    [Collection("RavenDb CommandStore")]
    public class RavenCommandStoreDuplicateMessageAsyncTests : IDisposable
    {
        private readonly RavenDbTestHelper helper;
        private readonly IAmACommandStoreAsync commandStore;
        private readonly MyCommand _raisedCommand;
        private MyCommand _storedCommand;

        public RavenCommandStoreDuplicateMessageAsyncTests()
        {
            helper = new RavenDbTestHelper();
            helper.CreateFreshTestDatabase();
            commandStore = new RavenDbCommandStore(new Raven.Client.Documents.DocumentStore()
            {
                Urls = RavenDbTestHelper.RavenDbSettings.NodeUrls,
                Database = RavenDbTestHelper.RavenDbSettings.TestDatabaseName
            });
            _raisedCommand = new MyCommand { Value = "Value" };
        }

        [Fact]
        public async Task When_The_Message_Is_Already_In_The_Command_Store_Async()
        {
            await commandStore.AddAsync(_raisedCommand);

            //this shouldn't throw
            await commandStore.AddAsync(_raisedCommand);            
        }

        public void Dispose()
        {
            helper.DeleteTestDatabase();
        }
    }

}
