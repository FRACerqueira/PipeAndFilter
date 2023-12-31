﻿using PipeFilterCore;

namespace PipeFilterCoreSamples
{
    class Program
    {
        public class MyClass
        {
            public int MyProperty { get; set; }
        }

        public static async Task<int> Main()
        {
            var contract = new MyClass { MyProperty = 10 };
            var pl = await PipeAndFilter.New<MyClass>()
                .AddPipe(ExecPipe)
                    .WithGotoCondition(CondFalse, "LastPipe")
                    .WithCondition(CondTrue)
                    .WithCondition(CondTrue)
                    .AfterRunningPipe(ExecPipeAfter)
                        .WithCondition(CondTrue)
                        .WithGotoCondition(CondFalse, "LastPipe")
                .AddPipe(ExecPipe100)
                    .AfterRunningAggregatePipe(ExecPipeAfterTask)
                        .MaxDegreeProcess(8)
                        .AddTaskCondition(Task50)
                            .WithCondition(CondTrue)
                        .AddTask(Task100)
                .AddAggregatePipe(AggregateTask)
                    .WithCondition(CondTrue)
                    .WithGotoCondition(CondFalse, "LastPipe")
                    .MaxDegreeProcess(4)
                    .AddTask(Task50)
                    .AddTaskCondition(Task100)
                        .WithCondition(CondTrue)
                        .WithCondition(CondFalse)
                   .AddTask(Task150)
                .AddAggregatePipe(AggregateTask)
                    .WithCondition(CondTrue)
                    .AddTaskCondition(Task100)
                        .WithCondition(CondTrue)
                    .AfterRunningAggregatePipe(ExecPipeAfterTask)
                        .MaxDegreeProcess(8)
                        .AddTaskCondition(Task50)
                            .WithCondition(CondTrue)
                        .AddTask(Task100)
                .AddAggregatePipe(AggregateTask)
                    .WithCondition(CondTrue)
                    .AddTaskCondition(Task100)
                        .WithCondition(CondTrue)
                .AddPipe(ExecPipe, "LastPipe")
                    .AfterRunningPipe(ExecPipeAfterTask)
                .BuildAndCreate()
                .Init(contract)
                .CorrelationId(null)
                .Logger(null)
                .Run();

            Console.WriteLine($"Contract value : {contract.MyProperty} Total Elapsedtime: {pl.Elapsedtime}");
            foreach (var item in pl.Status)
            {
                Console.WriteLine($"{item.Alias}:{item.Status.Value} Count: {item.Count} => {item.Status.Elapsedtime}");
                foreach (var det in item.StatusDetails)
                {
                    Console.WriteLine($"\t{det.TypeExec}:{det.GotoAlias ?? string.Empty}:{det.Alias ?? string.Empty}:{det.Condition}:{det.ToAliasCondition ?? string.Empty} => {det.Value}:{det.Elapsedtime} UTC:{det.DateRef:MM/dd/yyyy hh:mm:ss ffff}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key");
            Console.ReadKey();
            return 0;
        }

        private static async Task Task50(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.ThreadSafeAccess((contract) =>
            {
                contract.MyProperty++;
            });
            try
            {
                await Task.Delay(50, token);
                pipe.SaveValueAtEnd("T50",50);
            }
            catch (TaskCanceledException)
            {
                //none
            }
        }

        private static async Task Task100(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.ThreadSafeAccess((contract) =>
            {
                contract.MyProperty++;
            });
            try
            {
                await Task.Delay(100, token);
                pipe.SaveValueAtEnd("T100",100);
            }
            catch (TaskCanceledException)
            {
                //none
            }
        }

        private static async Task Task150(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.ThreadSafeAccess((contract) =>
            {
                contract.MyProperty++;
            });
            try
            {
                await Task.Delay(150, token);
                pipe.SaveValueAtEnd("T150",150);
            }
            catch (TaskCanceledException)
            {
                //none
            }
        }

        private static Task ExecPipe(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.SaveValueAtEnd("ExecPipe", "Saved1");
            return Task.CompletedTask;
        }

        private static Task ExecPipeAfter(EventPipe<MyClass> pipe, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        private static Task ExecPipeAfterTask(EventPipe<MyClass> pipe, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        private static Task AggregateTask(EventPipe<MyClass> pipe, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        private static async Task ExecPipe100(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.SaveValueAtEnd("ExecPipe100", "Saved2");
            try
            {
                await Task.Delay(100, token);
            }
            catch (TaskCanceledException)
            {
                //none
            }
        }

        private static async ValueTask<bool> CondFalse(EventPipe<MyClass> pipe, CancellationToken token)
        {
            return await Task.FromResult(false);
        }

        private static async ValueTask<bool> CondTrue(EventPipe<MyClass> pipe, CancellationToken token)
        {
            return await Task.FromResult(true);
        }
    }
}