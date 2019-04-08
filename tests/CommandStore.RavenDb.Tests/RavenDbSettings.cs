using System;
using System.Collections.Generic;
using System.Text;

namespace CommandStore.RavenDb.Tests
{
    public class RavenDbSettings
    {
        public string CommaSeperatedRavenDbNodeUrls { get; set; }

        public string[] NodeUrls => CommaSeperatedRavenDbNodeUrls.Split(',');

        public string TestDatabaseName {get; set;} = "CommandStoreTests";
    }
}
