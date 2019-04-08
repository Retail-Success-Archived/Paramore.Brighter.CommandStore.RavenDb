using System;
using Paramore.Brighter;
using Xunit;
using Paramore.Brighter.CommandStore.RavenDb;

namespace CommandStore.RavenDb.Tests
{
    [Trait("Category", "RavenDb")]
    [Collection("RavenDb CommandStore")]
    public class RavenCommandStoreDuplicateMessageTests : IDisposable
    {
        private readonly RavenDbTestHelper helper;
        private readonly IAmACommandStore commandStore;
        private readonly MyCommand _raisedCommand;
        private MyCommand _storedCommand;

        public RavenCommandStoreDuplicateMessageTests()
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
        public void When_The_Message_Is_Already_In_The_Command_Store()
        {
            commandStore.Add(_raisedCommand);

            //this shouldn't throw
            commandStore.Add(_raisedCommand);
        }

        public void Dispose()
        {
            helper.DeleteTestDatabase();
        }
    }

}
