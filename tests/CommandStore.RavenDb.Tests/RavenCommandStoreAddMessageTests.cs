using System;
using Paramore.Brighter;
using Xunit;
using Paramore.Brighter.CommandStore.RavenDb;
using FluentAssertions;

namespace CommandStore.RavenDb.Tests
{
    [Trait("Category", "RavenDb")]
    [Collection("RavenDb CommandStore")]
    public class RavenCommandStoreAddMessageTests : IDisposable
    {
        private readonly RavenDbTestHelper helper;
        private readonly IAmACommandStore commandStore;
        private readonly MyCommand _raisedCommand;
        private MyCommand _storedCommand;

        public RavenCommandStoreAddMessageTests()
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
        public void When_Writing_To_The_Command_StoreAsync()
        {
            commandStore.Add(_raisedCommand);

            _storedCommand = commandStore.Get<MyCommand>(_raisedCommand.Id);

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
