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

PipeAndFilter component for .NET Core with flexible conditions for each step (pipe)
and the ability to parallel execute tasks over a pipe.

Features
--------

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

Visit the official page for complete documentation of PipeAndFilter:
https://fracerqueira.github.io/PipeAndFilter

PipeAndFilter was developed in C# with target frameworks:

- netstandard2.1
- .NET 6
- .NET 7
- .NET 8

*** What's new in V1.0.4 ***
----------------------------

- Release G.A with .NET8 

**PipeAndFilter Sample-console Usage (Full features)**
------------------------------------------------------
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

**PipeAndFilter Sample-api/web Usage**
--------------------------------------

Program.cs
----------

builder.Services
    .AddPipeAndFilter(PipeAndFilter.New<WeatherForecast>()
        .AddPipe(TemperatureAdd10)
        .Build());
...

private static Task TemperatureAdd10(EventPipe<WeatherForecast> pipe, CancellationToken token)
{
    pipe.ThreadSafeAccess((contract) =>
    {
        contract.TemperatureC += 10;
    });
    return Task.CompletedTask;
}


WeatherForecastController.cs
----------------------------
    
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

**License**
-----------

Copyright 2023 @ Fernando Cerqueira
PipeAndFilter project is licensed under the  the MIT license.
https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE
