using System;
using Paramore.Brighter;
using Xunit;
using Paramore.Brighter.CommandStore.RavenDb;
using FluentAssertions;
using System.Threading.Tasks;

namespace CommandStore.RavenDb.Tests
{
    [Trait("Category", "RavenDb")]
    [Collection("RavenDb CommandStore")]
    public class RavenCommandStoreAddMessageAsyncTests : IDisposable
    {
        private readonly RavenDbTestHelper helper;
        private readonly IAmACommandStoreAsync commandStore;
        private readonly MyCommand _raisedCommand;
        private MyCommand _storedCommand;

        public RavenCommandStoreAddMessageAsyncTests()
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
        public async Task When_Writing_To_The_Command_StoreAsync()
        {
            await commandStore.AddAsync(_raisedCommand);

            _storedCommand = await commandStore.GetAsync<MyCommand>(_raisedCommand.Id);

            _storedCommand.Should().NotBeNull();
            _storedCommand.Value.Should().Be(_raisedCommand.Value);
            _storedCommand.Id.Should().Be(_raisedCommand.Id);
        }

        public void Dispose()
        {
            helper.DeleteTestDatabase();
        }
    }

}
