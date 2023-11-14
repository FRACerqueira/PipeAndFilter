# <img align="left" width="100" height="100" src="./docs/images/icon.png">Welcome to PipeAndFilter
[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)


### **PipeAndFilter component for .NET Core with flexible conditions for each step (pipe) and the ability to parallel execute tasks over a pipe.**

**PipeAndFilter** was developed in C# with the **netstandard2.1**, **.NET 6** , **.NET 7** and **.NET 8** target frameworks.


<img src="./docs/images/PipeAndFilterFeature.png"> 

**[Visit the official page for more documentation of PipeAndFilter](https://fracerqueira.github.io/PipeAndFilter)**

## Table of Contents

- [What's new - previous versions](./docs/whatsnewprev.md)
- [Features](#features)
- [Installing](#installing)
- [Examples](#examples)
- [Usage](#usage)
- [Performance](#performance)
- [Code of Conduct](#code-of-conduct)
- [Contributing](#contributing)
- [Credits](#credits)
- [License](#license)
- [API Reference](https://fracerqueira.github.io/PipeAndFilter/apis/apis.html)

## What's new in the latest version 
### V1.0.4 
[**Top**](#table-of-contents)

- Release G.A with .NET8 

## Features
[**Top**](#table-of-contents)

- Thread safety to obtain/change contract values ​​and/or generic purpose when running a Task (pararel execute)
- Add multiple pipe
- Add multiple Aggregate pipe (for run pararel tasks)
- Set the maximum amount of parallel execution
- Add multiple preconditions to run a pipe or task
- Add multiple link to the pipe to jump to another pipe
- Perform an action with conditions after pipe/aggregatepipe 
- Have detailed status (execution date, execution time, type of execution, result of each execution) and number of executions in each pipe
- Save multiple results from each pipe to be used during the another pipe/aggregate pipe run
- Save multiple results in each task to be effective during the aggregation pipe run
- Terminate the PipeAndFilter on any task, condition or pipe
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

The **PipeAndFilter** use **fluent interface**; an object-oriented API whose design relies extensively on method chaining. Its goal is to increase code legibility. The term was coined in 2005 by Eric Evans and Martin Fowler.

### Sample-Console Usage (Full features)

```csharp
await PipeAndFilter.New<MyClass>()
    .AddPipe(Pipe1)
        .WithGotoCondition(Cond0, "LastPipe")
        .WithCondition(Cond1)
        .WithCondition(Cond2)
        .AfterRunningPipe(ExecPipeAfter)
            .WithCondition(CondA1)
            .WithGotoCondition(CondA2, "LastPipe")
    .AddPipe(Pipe2)
        .AfterRunningPipe()
            .WithGotoCondition(CondA3, "LastPipe")
    .AddPipe(Pipe3)
        .AfterRunningAggregatePipe(ExecPipeAfterTask)
            .MaxDegreeProcess(8)
            .AddTaskCondition(Task50)
                .WithCondition(CondTrue)
            .AddTask(Task100)    
    .AddPipe(Pipe4)
    .AddAggregatePipe(Pipe5)
        .WithCondition(Cond1)
        .MaxDegreeProcess(4)
        .AddTask(Task50)
        .AddTaskCondition(Task100)
            .WithCondition(Cond3)
            .WithCondition(Cond4)
        .AddTask(Task150)
    .AddPipe(Pipe6, "LastPipe")
    .BuildAndCreate()
    .Init(contract)
    .CorrelationId(null)
    .Logger(null)
    .Run();
```

### Sample-api/webUsage
[**Top**](#table-of-contents)

```csharp
builder.Services
    .AddPipeAndFilter(PipeAndFilter.New<WeatherForecast>()
        .AddPipe(TemperatureAdd10)
        .Build());
```

```csharp
private static Task TemperatureAdd10(EventPipe<WeatherForecast> pipe, CancellationToken token)
{
    pipe.ThreadSafeAccess((contract) =>
    {
        contract.TemperatureC += 10;
    });
    return Task.CompletedTask;
}
```

```csharp
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IPipeAndFilterService<WeatherForecast> _mypipe;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IPipeAndFilterService<WeatherForecast> pipeAndFilter)
    {
        _logger = logger;
        _mypipes = pipeAndFilter;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<WeatherForecast> Get(CancellationToken cancellation)
    {
            var cid = Guid.NewGuid().ToString();

            var pipe = await _mypipes
                .Create()
                .Logger(_logger)
                .CorrelationId(cid)
                .Init(new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now), Summary = "PipeAndFilter-Opc1", TemperatureC = 0 })
                .Run(cancellation);
            return pipe.Value!
    }
}
```

## Performance
[**Top**](#table-of-contents)

All pipes, conditions and tasks do not perform any task, they are only called and executed by the component

See folder [**Samples/PipeandFIlterBenchmarking**](https://github.com/FRACerqueira/PipeAndFilter/tree/main/Samples/PipeandFIlterBenchmarking).

```
BenchmarkDotNet v0.13.10, Windows 10 (10.0.19044.3570/21H2/November2021Update)
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method                       | Mean       | Error     | StdDev     | Median     | Gen0    | Allocated |
|----------------------------- |-----------:|----------:|-----------:|-----------:|--------:|----------:|
| PipeAsync                    |   6.043 us | 0.1453 us |  0.4240 us |   5.929 us |  0.9842 |   4.03 KB |
| PipeWith10Async              | 219.920 us | 9.0387 us | 26.2230 us | 213.204 us | 20.7520 |  85.45 KB |
| PipeWithConditionAsync       |   8.561 us | 0.1803 us |  0.5203 us |   8.458 us |  1.2970 |    5.3 KB |
| PipeWith10ConditionAsync     |  54.716 us | 2.4453 us |  7.2099 us |  51.169 us |  6.4697 |  26.84 KB |
| PipeWith10ConditionGotoAsync |  88.367 us | 2.2634 us |  6.4942 us |  88.401 us |  8.4229 |  34.75 KB |
| PipeTaskAsync                |  16.670 us | 0.3316 us |  0.9297 us |  16.466 us |  1.5869 |   6.49 KB |
| PipeWith10TaskAsync          | 179.862 us | 5.2544 us | 14.9910 us | 175.589 us |  7.8125 |  32.68 KB |
| PipeTaskConditionAsync       |  20.225 us | 0.4035 us |  1.0344 us |  20.082 us |  1.8921 |   7.73 KB |
| PipeWith10TaskConditionAsync | 183.540 us | 4.5200 us | 13.1851 us | 180.380 us |  7.8125 |  32.68 KB |
```

```
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2


| Method                       | Mean       | Error      | StdDev     | Median     | Gen0    | Allocated |
|----------------------------- |-----------:|-----------:|-----------:|-----------:|--------:|----------:|
| PipeAsync                    |   7.779 us |  0.3361 us |  0.9752 us |   7.439 us |  1.0071 |   4.13 KB |
| PipeWith10Async              | 233.087 us | 11.3821 us | 33.2020 us | 221.658 us | 20.9961 |  86.47 KB |
| PipeWithConditionAsync       |  10.678 us |  0.2496 us |  0.7122 us |  10.448 us |  1.3428 |   5.51 KB |
| PipeWith10ConditionAsync     |  72.872 us |  2.1928 us |  6.1489 us |  71.057 us |  6.8359 |  27.95 KB |
| PipeWith10ConditionGotoAsync |  94.723 us |  1.8764 us |  4.6729 us |  93.935 us |  8.7891 |  35.98 KB |
| PipeTaskAsync                |  20.262 us |  0.5102 us |  1.4555 us |  19.804 us |  1.6174 |   6.64 KB |
| PipeWith10TaskAsync          | 252.078 us |  4.9050 us |  6.7141 us | 250.701 us |  7.8125 |  33.21 KB |
| PipeTaskConditionAsync       |  28.370 us |  0.9535 us |  2.7662 us |  28.128 us |  1.9531 |   7.99 KB |
| PipeWith10TaskConditionAsync | 257.651 us |  5.1477 us |  6.5102 us | 260.074 us |  7.8125 |  33.21 KB |
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

