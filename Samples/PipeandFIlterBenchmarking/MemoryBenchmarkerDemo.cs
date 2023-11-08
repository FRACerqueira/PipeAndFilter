using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using PipeFilterCore;

namespace PipeandFIlterBenchmarking
{
    [MemoryDiagnoser]
    public class MemoryBenchmarkerDemo
    {
        public class MyClass
        {
            public int MyProperty { get; set; }
        }

        [Benchmark]
        public async Task PipeAsync()
        {
            await PipeAndFilter.New<MyClass>()
                .AddPipe(ExecPipe)
                .BuildAndCreate()
                .Run();
        }

        [Benchmark]
        public async Task PipeWith10Async()
        {
            var aux = PipeAndFilter.New<MyClass>();
            for (int i = 0; i < 10; i++)
            {
                aux.AddPipe(ExecPipe);
            }
            await aux.BuildAndCreate()
                .Run();
        }

        [Benchmark]
        public async Task PipeWithConditionAsync()
        {
            await PipeAndFilter.New<MyClass>()
                .AddPipe(ExecPipe)
                    .WithCondition(CondTrue)
                .BuildAndCreate()
                .Run();
        }

        [Benchmark]
        public async Task PipeWith10ConditionAsync()
        {
            var aux = PipeAndFilter.New<MyClass>()
                .AddPipe(ExecPipe);
            for (int i = 0; i < 10; i++)
            {
                aux.WithCondition(CondTrue);
           }
            await aux.BuildAndCreate()
                .Run();
        }

        [Benchmark]
        public async Task PipeWith10ConditionGotoAsync()
        {
            var aux = PipeAndFilter.New<MyClass>()
                .AddPipe(ExecPipe);
            for (int i = 0; i < 10; i++)
            {
                aux.WithGotoCondition(CondFalse, "EndPipe");
            };
            aux.AddPipe(ExecPipe,"EndPipe");
            await aux.BuildAndCreate()
                .Run();
        }

        [Benchmark]
        public async Task PipeTaskAsync()
        {
            await PipeAndFilter.New<MyClass>()
                .AddPipeTasks(ExecPipe)
                    .AddTask(ExecTask)
                .BuildAndCreate()
                .Run();
        }


        [Benchmark]
        public async Task PipeWith10TaskAsync()
        {
            var aux = PipeAndFilter.New<MyClass>()
                .AddPipeTasks(ExecPipe)
                .MaxDegreeProcess(4);
            for (int i = 0; i < 10; i++)
            {
                aux.AddTask(ExecTask);
            }          
            await aux.BuildAndCreate()
                .Run();
        }


        [Benchmark]
        public async Task PipeTaskConditionAsync()
        {
            await PipeAndFilter.New<MyClass>()
                .AddPipeTasks(ExecPipe)
                    .AddTaskCondition(ExecTask,CondTrue)
                .BuildAndCreate()
                .Run();
        }

        [Benchmark]
        public async Task PipeWith10TaskConditionAsync()
        {
            var aux = PipeAndFilter.New<MyClass>()
                .AddPipeTasks(ExecPipe)
                .MaxDegreeProcess(4);
            for (int i = 0; i < 10; i++)
            {
                aux.AddTask(ExecTask);
            }
            await aux.BuildAndCreate()
                .Run();
        }


        private async Task ExecTask(EventPipe<MyClass> pipe, CancellationToken token)
        {
            await Task.CompletedTask;
        }
        private async Task ExecPipe(EventPipe<MyClass> pipe, CancellationToken token)
        {
            await Task.CompletedTask;
        }

        private async ValueTask<bool> CondFalse(EventPipe<MyClass> pipe, CancellationToken token)
        {
            return await Task.FromResult(false);
        }

        private async ValueTask<bool> CondTrue(EventPipe<MyClass> pipe, CancellationToken token)
        {
            return await Task.FromResult(true);
        }
    }
}
