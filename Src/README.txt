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

Key features
------------
- Contract with thread safety for change values
- Set the maximum amount of parallel execution
- Add multiple preconditions to run a pipe
- Add multiple link to the pipe to jump to another pipe
- Add tasks with a precondition
- Have the detailed status (execution time, execution type, result of each executed condition) in each pipe
- Save a result from each pipe to use when executing another pipe
- Save a result from each task to use during the execution of the aggregation pipe
- Terminate the PipeAndFilter on any task, condition or pipe
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

**PipeAndFilter Sample-console Usage**
--------------------------------------
var result = await PipeAndFilter.New<MyClass>()
    .AddPipe(ExecPipe1)
        .WithCondition(CondFalse, "LastPipe")
        .WithCondition(CondTrue, null)
        .WithCondition(CondTrue, null)
    .AddPipe(ExecPipe2)
    .AddPipe(ExecPipe3)
    .AddPipeTasks(AgregateTask)
        .WithCondition(CondTrue, null)
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

**PipeAndFilter Sample-api/web Usage**
--------------------------------------

Program.cs
----------

builder.Services
    .AddPipeAndFilter(
        PipeAndFilter.New<WeatherForecast>()
            .AddPipe(ExecPipe)
            .Build());
...
...

private static Task ExecPipe(EventPipe<WeatherForecast> pipe, CancellationToken token)
{
    pipe.ChangeContract((contract) =>
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

            var pipe = await _mypipes.First(x => x.ServiceId == "opc1")
                .Create()
                .Logger(_logger)
                .CorrelationId(cid)
                .Init(new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Now), Summary = "PipeAndFilter-Opc1", TemperatureC = 0 })
                .Run(cancellation);
            return pipe.Result.Value!
    }
}

**License**
-----------

Copyright 2023 @ Fernando Cerqueira
PipeAndFilter project is licensed under the  the MIT license.
https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE