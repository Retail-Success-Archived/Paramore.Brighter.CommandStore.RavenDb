using System;
using Paramore.Brighter;

namespace CommandStore.RavenDb.Tests
{
    public class MyCommand : Command
    {
        public MyCommand() : base(Guid.NewGuid())
        {

        }

        public string Value {get; set;}
    }
}