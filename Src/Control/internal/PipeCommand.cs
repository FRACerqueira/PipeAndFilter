using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeFilterCore
{
    internal readonly struct PipeCommand<T> where T : class
    {
        public PipeCommand(string id, Func<EventPipe<T>, CancellationToken, Task> handler) 
        {
            Id = id;
            Handler = handler;
        }
        public string Id { get; }
        public Func<EventPipe<T>, CancellationToken, Task> Handler { get; }
    }
}
