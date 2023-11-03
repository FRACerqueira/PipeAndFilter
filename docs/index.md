# <img align="left" width="100" height="100" src="./images/icon.png">Welcome to PipeAndFilter
[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)


**Pipeline control for .NET Core with flexible conditions for each step (pipe)
and the ability to parallel execute tasks over a pipe.
.**

**PipeAndFilter** was developed in C# with the **netstandard2.1**, **.NET 6** and **.NET 7** target frameworks.
**[Visit the official page for more documentation of PipeAndFilter](https://fracerqueira.github.io/PipeAndFilter)**

## Table of Contents

- [What's new - previous versions]()
- [Features](#features)
- [Installing](#installing)
- [Examples](#examples)
- [Usage](#usage)
- [Code of Conduct](#code-of-conduct)
- [Contributing](#contributing)
- [Credits](#credits)
- [License](#license)
- [API Reference](https://fracerqueira.github.io/PipeAndFilter/apis/apis.html)

## What's new in the latest version 
### V1.0.0 

[**Top**](#table-of-contents)

- First Release

## Features
[**Top**](#table-of-contents)

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

## Installing
[**Top**](#table-of-contents)

```
Install-Package PipeAndFilter [-pre]
```

```
dotnet add package PipeAndFilter [--prerelease]
```

**_Note:  [-pre]/[--prerelease] usage for pre-release versions_**

## Examples
[**Top**](#table-of-contents)

See folder [**Samples**](https://github.com/FRACerqueira/PipeAndFilter/tree/main/Samples).

```
dotnet run --project [name of sample]
```

## Usage
[**Top**](#table-of-contents)

The controls use **fluent interface**; an object-oriented API whose design relies extensively on method chaining. Its goal is to increase code legibility. The term was coined in 2005 by Eric Evans and Martin Fowler.
```csharp
public class MyClass
{
    public int MyProperty { get; set; }
}
```

```csharp
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
```

```csharp
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
```

## Code of Conduct
[**Top**](#table-of-contents)

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [Code of Conduct](CODE_OF_CONDUCT.md).

## Contributing

See the [Contributing guide](CONTRIBUTING.md) for developer documentation.

## Credits
[**Top**](#table-of-contents)

**API documentation generated by**

- [xmldoc2md](https://github.com/FRACerqueira/xmldoc2md), Copyright (c) 2022 Charles de Vandière. See [LICENSE](Licenses/LICENSE-xmldoc2md.md).

## License
[**Top**](#table-of-contents)

Copyright 2023 @ Fernando Cerqueira

PipeAndFilter is licensed under the MIT license. See [LICENSE](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE).
