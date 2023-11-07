# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterTasksService<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterTasksService&lt;T&gt;

Namespace: PipeFilterCore

Represents commands for task.

```csharp
public interface IPipeAndFilterTasksService<T> : IPipeAndFilterBuild<T>
```

#### Type Parameters

`T`<br>
Type of contract.

Implements IPipeAndFilterBuild&lt;T&gt;

## Methods

### <a id="methods-addpipe"/>**AddPipe(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new pipe.

```csharp
IPipeAndFilterService<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler pipe to execute.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe.
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).<br>Alias ​​is used to reference in another pipe.

#### Returns

[IPipeAndFilterService&lt;T&gt;](./pipefiltercore.ipipeandfilterservice-1.md)

### <a id="methods-addpipetasks"/>**AddPipeTasks(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new pipe aggregate tasks.

```csharp
IPipeAndFilterTasksService<T> AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler pipe aggregate to execute.
 <br>The handler command will run after all tasks are executed.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe.
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).<br>Alias ​​is used to reference in another pipe.

#### Returns

[IPipeAndFilterTasksService&lt;T&gt;](./pipefiltercore.ipipeandfiltertasksservice-1.md)

### <a id="methods-addtask"/>**AddTask(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new task (execution in parallel) through pipe.

```csharp
IPipeAndFilterTasksService<T> AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string nametask)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler task to execute.

`nametask` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for task (optional).

#### Returns

[IPipeAndFilterTasksService&lt;T&gt;](./pipefiltercore.ipipeandfiltertasksservice-1.md)

### <a id="methods-addtaskcondition"/>**AddTaskCondition(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;, String, String)**

Add new task (execution in parallel) through pipe with a condition.

```csharp
IPipeAndFilterTasksService<T> AddTaskCondition(Func<EventPipe<T>, CancellationToken, Task> command, Func<EventPipe<T>, CancellationToken, ValueTask<Boolean>> condition, string nametask, string namecondition)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler task to execute.

`condition` Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;<br>
The handler task to condition.

`nametask` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for task (optional).

`namecondition` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for condition (optional).

#### Returns

[IPipeAndFilterTasksService&lt;T&gt;](./pipefiltercore.ipipeandfiltertasksservice-1.md)

### <a id="methods-maxdegreeprocess"/>**MaxDegreeProcess(Int32)**

Maximum number of concurrent tasks enable.

```csharp
IPipeAndFilterTasksService<T> MaxDegreeProcess(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of concurrent tasks.
 <br>The default value is number of processors.

#### Returns

[IPipeAndFilterTasksService&lt;T&gt;](./pipefiltercore.ipipeandfiltertasksservice-1.md)

### <a id="methods-withcondition"/>**WithCondition(Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;, String, String)**

Add new condition.

```csharp
IPipeAndFilterTasksService<T> WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<Boolean>> condition, string aliasgoto, string namecondition)
```

#### Parameters

`condition` Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;<br>
The handle condition to execute.

`aliasgoto` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias to another pipe.
 <br>If condition not have link to another pipe, the value must be null.

`namecondition` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for condition(optional).

#### Returns

[IPipeAndFilterTasksService&lt;T&gt;](./pipefiltercore.ipipeandfiltertasksservice-1.md)


- - -
[**Back to List Api**](./apis.md)
