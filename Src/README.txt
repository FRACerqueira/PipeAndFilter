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
- Thread safety to obtain/change contract values ​​and/or generic purpose when running a Task (pararel execute)
- Add multiple pipe
- Add multiple agregate pipe (for run pararel tasks)
- Set the maximum amount of parallel execution
- Add multiple preconditions to run a pipe or pipe
- Add multiple link to the pipe to jump to another pipe
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

*** What's new in V1.0.2 ***
----------------------------

- Added ability to save/overwrite multiple result to use during the execution another pipe / aggregation pipe
    - Removed propery 'SavedTasks' in EventPipe
    - Removed propery 'SavedPipes' in EventPipe
    - Removed Method 'SaveValue'
    - Removed Method 'RemoveSavedValue'
    - Added Method TrySavedValue
        - Now TrySavedValue return true/false if exist id saved and value in out paramameter
    - Added Method SaveValueAtEnd
        - Now SaveValueAtEnd receives the unique id to be saved/overwrite and the value
    - Added Method RemoveValueAtEnd
        - Now RemoveValueAtEnd receives the unique id to be removed if any
- Added ability to multiple preconditions for Tasks
    - Channged command AddTaskCondition
        - Now the same parameters as AddTask
    - Added command WithCondition for AddTaskCondition

**PipeAndFilter Sample-console Usage**
--------------------------------------
await PipeAndFilter.New<MyClass>()
    .AddPipe(Pipe1)
        .WithGotoCondition(Cond0, "LastPipe")
        .WithCondition(Cond1)
        .WithCondition(Cond2)
    .AddPipe(Pipe2)
    .AddPipe(Pipe3)
    .AddPipeTasks(Pipe4)
        .WithCondition(Cond1)
        .MaxDegreeProcess(4)
        .AddTask(Task50)
        .AddTaskCondition(Task100)
            .WithCondition(Cond3)
            .WithCondition(Cond4)
        .AddTask(Task150)
    .AddPipe(Pipe5, "LastPipe")
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
