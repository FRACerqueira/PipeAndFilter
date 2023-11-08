# <img align="left" width="100" height="100" src="./docs/images/icon.png">Welcome to PipeAndFilter
[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)


### **PipeAndFilter component for .NET Core with flexible conditions for each step (pipe) and the ability to parallel execute tasks over a pipe.**

**PipeAndFilter** was developed in C# with the **netstandard2.1**, **.NET 6** and **.NET 7** target frameworks.

<img src="./docs/images/PipeAndFilterFeature.png"> 

**[Visit the official page for more documentation of PipeAndFilter](https://fracerqueira.github.io/PipeAndFilter)**

## Table of Contents

- [What's new - previous versions]()
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
### V1.0.0 

[**Top**](#table-of-contents)

- First Release

## Features
[**Top**](#table-of-contents)

- Contract with thread safety for change values
- Set the maximum amount of parallel execution
- Add multiple preconditions to run a pipe
- Add multiple link to the pipe to jump to another pipe
- Add tasks with a precondition
- Have detailed status (execution date, execution time, type of execution, result of each execution) and number of executions in each pipe
- Save a result from each pipe to use when executing another pipe
- Save a result from each task to use during the execution of the aggregation pipe
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

### Sample-Console Usage

```csharp
var result = await PipeAndFilter.New<MyClass>()
    .AddPipe(ExecPipe1)
        .WithGotoCondition(CondFalse, "LastPipe")
        .WithCondition(CondTrue)
        .WithCondition(CondTrue)
    .AddPipe(ExecPipe2)
    .AddPipe(ExecPipe3)
    .AddPipeTasks(AgregateTask)
        .WithCondition(CondTrue)
        .MaxDegreeProcess(4)
        .AddTask(Task1)
        .AddTaskCondition(Task2, CondFalse)
        .AddTask(Task3)
    .AddPipe(ExecPipe5, "LastPipe")
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
        .AddPipe(ExecPipe)
        .Build());
```

```csharp
private static Task ExecPipe(EventPipe<WeatherForecast> pipe, CancellationToken token)
{
    pipe.ChangeContract((contract) =>
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
    private readonly IPipeAndFilterServiceBuild<WeatherForecast> _mypipe;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IPipeAndFilterServiceBuild<WeatherForecast> pipeAndFilter)
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
.NET SDK 8.0.100-rc.2.23502.2
  [Host]     : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2
```

| Method                       | Mean       | Error      | StdDev     | Median     | Gen0    | Allocated |
|----------------------------- |-----------:|-----------:|-----------:|-----------:|--------:|----------:|
| PipeAsync                    |   7.419 us |  0.1483 us |  0.3347 us |   7.345 us |  1.1597 |   4.74 KB |
| PipeWith10Async              | 239.257 us | 10.6802 us | 30.9852 us | 234.596 us | 19.5313 |  80.68 KB |
| PipeWithConditionAsync       |   8.273 us |  0.1639 us |  0.2599 us |   8.146 us |  1.4038 |   5.76 KB |
| PipeWith10ConditionAsync     |  20.606 us |  0.4113 us |  0.9774 us |  20.202 us |  3.6011 |  14.78 KB |
| PipeWith10ConditionGotoAsync |  33.396 us |  0.6631 us |  1.2455 us |  33.024 us |  5.1270 |  21.08 KB |
| PipeTaskAsync                |  16.918 us |  0.5232 us |  1.5096 us |  16.795 us |  1.7090 |   7.07 KB |
| PipeWith10TaskAsync          |  72.402 us |  3.5790 us |  9.8577 us |  68.424 us |  4.8828 |  20.47 KB |
| PipeTaskConditionAsync       |  19.375 us |  0.3853 us |  0.9736 us |  19.425 us |  1.8616 |   7.66 KB |
| PipeWith10TaskConditionAsync |  63.898 us |  1.2774 us |  2.9858 us |  63.562 us |  4.8828 |  20.47 KB |

```
BenchmarkDotNet v0.13.10, Windows 10 (10.0.19044.3570/21H2/November2021Update)
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100-rc.2.23502.2
  [Host]     : .NET 7.0.13 (7.0.1323.51816), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.13 (7.0.1323.51816), X64 RyuJIT AVX2
```

| Method                       | Mean       | Error     | StdDev     | Median     | Gen0    | Allocated |
|----------------------------- |-----------:|----------:|-----------:|-----------:|--------:|----------:|
| PipeAsync                    |  11.231 μs | 0.4573 μs |  1.3195 μs |  10.898 μs |  1.1597 |   4.79 KB |
| PipeWith10Async              | 226.285 μs | 4.5187 μs | 10.8264 μs | 222.330 μs | 19.5313 |  80.73 KB |
| PipeWithConditionAsync       |   9.902 μs | 0.1137 μs |  0.0950 μs |   9.858 μs |  1.4038 |    5.8 KB |
| PipeWith10ConditionAsync     |  26.949 μs | 0.9860 μs |  2.6824 μs |  26.154 μs |  3.6011 |  14.83 KB |
| PipeWith10ConditionGotoAsync |  39.820 μs | 0.7613 μs |  1.2075 μs |  39.498 μs |  5.1270 |  21.13 KB |
| PipeTaskAsync                |  20.286 μs | 0.6041 μs |  1.7334 μs |  19.744 μs |  1.7395 |   7.12 KB |
| PipeWith10TaskAsync          | 101.252 μs | 5.3239 μs | 15.4455 μs |  97.842 μs |  4.8828 |  20.53 KB |
| PipeTaskConditionAsync       |  24.214 μs | 1.3098 μs |  3.7998 μs |  22.740 μs |  1.8616 |    7.7 KB |
| PipeWith10TaskConditionAsync |  98.953 μs | 3.9903 μs | 11.0570 μs |  95.221 μs |  4.8828 |  20.56 KB |

```
BenchmarkDotNet v0.13.10, Windows 10 (10.0.19044.3570/21H2/November2021Update)
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100-rc.2.23502.2
  [Host]     : .NET Core 3.1.32 (CoreCLR 4.700.22.55902, CoreFX 4.700.22.56512), X64 RyuJIT AVX2
  DefaultJob : .NET Core 3.1.32 (CoreCLR 4.700.22.55902, CoreFX 4.700.22.56512), X64 RyuJIT AVX2
```

| Method                       | Mean      | Error    | StdDev   | Gen0    | Allocated |
|----------------------------- |----------:|---------:|---------:|--------:|----------:|
| PipeAsync                    |  14.38 us | 0.225 us | 0.199 us |  1.1597 |   4.77 KB |
| PipeWith10Async              | 328.18 us | 6.287 us | 5.880 us | 19.5313 |   81.7 KB |
| PipeWithConditionAsync       |  17.11 us | 0.283 us | 0.237 us |  1.4038 |   5.85 KB |
| PipeWith10ConditionAsync     |  36.82 us | 0.702 us | 0.657 us |  3.6621 |  15.44 KB |
| PipeWith10ConditionGotoAsync |  58.69 us | 1.103 us | 2.557 us |  5.3101 |   21.8 KB |
| PipeTaskAsync                |  37.06 us | 0.720 us | 1.077 us |  1.7090 |   7.18 KB |
| PipeWith10TaskAsync          | 222.57 us | 2.935 us | 2.745 us |  4.8828 |  20.52 KB |
| PipeTaskConditionAsync       |  42.97 us | 0.906 us | 2.525 us |  1.8921 |   7.84 KB |
| PipeWith10TaskConditionAsync | 224.77 us | 2.119 us | 1.982 us |  4.8828 |  20.54 KB |

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

