============================================================================
 ____   _                  _                _  _____  _  _  _
|  _ \ (_) _ __    ___    / \    _ __    __| ||  ___|(_)| || |_   ___  _ __
| |_) || || '_ \  / _ \  / _ \  | '_ \  / _` || |_   | || || __| / _ \| '__|
|  __/ | || |_) ||  __/ / ___ \ | | | || (_| ||  _|  | || || |_ |  __/| |
|_|    |_|| .__/  \___|/_/   \_\|_| |_| \__,_||_|    |_||_| \__| \___||_|
          |_|

============================================================================

Welcome to PipeAndFilter
------------------------

Pipeline control for .NET Core with flexible conditions for each step (pipe)
and the ability to parallel execute tasks over a pipe.

Key features
------------
- Pipeline contract with thread safety for change values
- Set the maximum amount of parallel execution
- Add multiple preconditions to run a pipe
- Add multiple link to the pipe to jump to another pipe
- Add tasks with a precondition
- Have the detailed status (execution time, execution type, result of each executed condition) in each pipe
- Save a result from each pipe to use when executing another pipe
- Save a result from each task to use during the execution of the aggregation pipe
- Terminate the pipeline on any task, condition or pipe
- Simple and clear fluent syntax

Visit the official page for complete documentation of PipeAndFilter:
https://fracerqueira.github.io/PipeAndFilter

PipeAndFilter was developed in C# with target frameworks:

- netstandard2.1
- .NET 6
- .NET 7

*** What's new in V1.0.0 ***
----------------------------

- First Release

**PipeAndFilter Controls - Sample Usage**
-----------------------------------------

public class MyClass
{
    public int MyProperty { get; set; }
}

...

var contract = new MyClass { MyProperty = 10 };

var result = await Pipeline
    .Create<MyClass>()
    .Init(contract)
    .MaxDegreeProcess(4)
    .AddPipe(ExecPipe1)
    .AddPipe(ExecPipe2)
        .WithCondition(CondFalse, "LastPipe")
        .WithCondition(CondTrue, null)
        .WithCondition(CondTrue, null)
     .AddPipeTasks(AgregateTask)
        .WithCondition(CondTrue, null)
        .AddTask(Task50)
        .AddTaskCondition(Task100, CondFalse)
        .AddTask(Task150)
     .AddPipe(ExecPipe, "LastPipe")
     .Run();

Console.WriteLine($"Contract value : {contract.MyProperty}");
foreach (var item in pl.Status)
{
    Console.WriteLine($"{item.Alias ?? item.Id}:{item.Status.Value} => {item.Status.Elapsedtime}");
    foreach (var det in item.StatusDetails)
    {
        Console.WriteLine($"\t{det.TypeExec}:{det.GotoAlias ?? det.Alias}:{det.Condition} => :{det.Value}:{det.Elapsedtime}");
    }
}

...

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
private static ValueTask<bool> CondTrue(EventPipe<MyClass> pipe, CancellationToken token)
{
    ValueTask.FromResult(true);
}

**License**
-----------

Copyright 2023 @ Fernando Cerqueira
PipeAndFilter project is licensed under the  the MIT license.
https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE