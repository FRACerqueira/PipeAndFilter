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
                    .WithCondition(CondFalse, "LastPipe")
                    .WithCondition(CondTrue, null)
                    .WithCondition(CondTrue, null)
                .AddPipe(ExecPipe)
                .AddPipe(ExecPipe100)
                .AddPipeTasks(AgregateTask)
                    .WithCondition(CondTrue, null)
                    .MaxDegreeProcess(4)
                    .AddTask(Task50)
                    .AddTaskCondition(Task100, CondFalse)
                    .AddTask(Task150)
                .AddPipe(ExecPipe, "LastPipe")
                .BuildAndCreate()
                .Init(contract)
                .CorrelationId(null)
                .Logger(null)
                .Run();

            Console.WriteLine($"Contract value : {contract.MyProperty} Total Elapsedtime: {pl.Elapsedtime}" );
            foreach (var item in pl.Status)
            {
                Console.WriteLine($"{item.Alias}:{item.Status.Value} Count: {item.Count} => {item.Status.Elapsedtime}");
                foreach (var det in item.StatusDetails) 
                {
                    Console.WriteLine($"\t{det.TypeExec}:{det.GotoAlias ?? det.Alias}:{det.Condition} => {det.Value}:{det.Elapsedtime} UTC:{det.DateRef.ToString("MM/dd/yyyy hh:mm:ss ffff")}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key");
            Console.ReadKey();
            return 0;
        }

        private static async Task Task50(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.ChangeContract((contract) =>
            {
                contract.MyProperty++;
            });
            try
            {
                await Task.Delay(50, token);
                pipe.SaveValue(50);
            }
            catch (TaskCanceledException)
            {
                //none
            }
        }

        private static async Task Task100(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.ChangeContract((contract) =>
            {
                contract.MyProperty++;
            });
            try
            {
                await Task.Delay(100, token);
                pipe.SaveValue(100);
            }
            catch (TaskCanceledException)
            {
                //none
            }
        }

        private static async Task Task150(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.ChangeContract((contract) =>
            {
                contract.MyProperty++;
            });
            try
            {
                await Task.Delay(150, token);
                pipe.SaveValue(150);
            }
            catch (TaskCanceledException)
            {
                //none
            }
        }

        private static Task ExecPipe(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.SaveValue("Saved");
            return Task.CompletedTask;
        }

        private static Task AgregateTask(EventPipe<MyClass> pipe, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        private static async Task ExecPipe100(EventPipe<MyClass> pipe, CancellationToken token)
        {
            pipe.SaveValue("Saved0");
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