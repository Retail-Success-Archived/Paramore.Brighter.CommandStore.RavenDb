using System;

namespace Paramore.Brighter.CommandStore.RavenDb
{
    public class Command<T>
    {
        public string Id { get; set; }
        public Guid CommandId { get; set; }
        public string CommandType { get; set; }
        public T CommandBody { get; set; }
        public DateTime? Timestamp { get; set; }        
    }
}
