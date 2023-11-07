using BenchmarkDotNet.Running;

namespace PipeandFIlterBenchmarking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<MemoryBenchmarkerDemo>();
        }
    }
}