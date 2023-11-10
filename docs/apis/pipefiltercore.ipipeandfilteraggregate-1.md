# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterAggregate<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterAggregate&lt;T&gt;

Namespace: PipeFilterCore

Represents commands task for Aggregate pipe.

```csharp
public interface IPipeAndFilterAggregate<T> : IPipeAndFilterBuild<T>
```

#### Type Parameters

`T`<br>
Type of contract.

Implements IPipeAndFilterBuild&lt;T&gt;

## Methods

### <a id="methods-addaggregatepipe"/>**AddAggregatePipe(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new aggregate pipe.

```csharp
IPipeAndFilterAggregate<T> AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler to execute.
 <br>The handler command will run after all tasks are executed.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe.
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).<br>Alias ​​is used to reference in another pipe.

#### Returns

[IPipeAndFilterAggregate&lt;T&gt;](./pipefiltercore.ipipeandfilteraggregate-1.md)

### <a id="methods-addpipe"/>**AddPipe(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new pipe.

```csharp
IPipeAndFilterPipe<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler to execute.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe.
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).<br>Alias ​​is used to reference in another pipe.

#### Returns

[IPipeAndFilterPipe&lt;T&gt;](./pipefiltercore.ipipeandfilterpipe-1.md)

### <a id="methods-addtask"/>**AddTask(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new task (execution in parallel) to the aggregate pipe.

```csharp
IPipeAndFilterAggregate<T> AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string nametask)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler to execute.

`nametask` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for task (optional).

#### Returns

[IPipeAndFilterAggregate&lt;T&gt;](./pipefiltercore.ipipeandfilteraggregate-1.md)

### <a id="methods-addtaskcondition"/>**AddTaskCondition(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new task (execution in parallel) with conditions to the aggregate pipe.

```csharp
IPipeAndFilterAggregateCondition<T> AddTaskCondition(Func<EventPipe<T>, CancellationToken, Task> command, string nametask)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler to execute.

`nametask` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for task (optional).

#### Returns

[IPipeAndFilterAggregateCondition&lt;T&gt;](./pipefiltercore.ipipeandfilteraggregatecondition-1.md)

### <a id="methods-afterrunningaggregatepipe"/>**AfterRunningAggregatePipe(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new aggregate pipe to run after aggregate pipe completes

```csharp
IPipeAndFilterAfterAggregate<T> AfterRunningAggregatePipe(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler to execute.
 <br>The handler command will run after all tasks are executed.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe.
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).<br>Alias ​​is used to reference in another pipe.

#### Returns

[IPipeAndFilterAfterAggregate&lt;T&gt;](./pipefiltercore.ipipeandfilterafteraggregate-1.md)

### <a id="methods-afterrunningpipe"/>**AfterRunningPipe(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new pipe to run after aggregate pipe completes.

```csharp
IPipeAndFilterAfterPipe<T> AfterRunningPipe(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler to execute.
 <br>The handler command will run after all tasks are executed.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe.
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).<br>Alias ​​is used to reference in another pipe.

#### Returns

[IPipeAndFilterAfterPipe&lt;T&gt;](./pipefiltercore.ipipeandfilterafterpipe-1.md)

### <a id="methods-maxdegreeprocess"/>**MaxDegreeProcess(Int32)**

Maximum number of concurrent tasks enable.

```csharp
IPipeAndFilterAggregate<T> MaxDegreeProcess(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of concurrent tasks.
 <br>The default value is number of processors.

#### Returns

[IPipeAndFilterAggregate&lt;T&gt;](./pipefiltercore.ipipeandfilteraggregate-1.md)

### <a id="methods-withcondition"/>**WithCondition(Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;, String)**

Add new condition to the aggregate pipe.

```csharp
IPipeAndFilterAggregate<T> WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<Boolean>> condition, string namecondition)
```

#### Parameters

`condition` Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;<br>
The handle to execute.

`namecondition` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for condition(optional).

#### Returns

[IPipeAndFilterAggregate&lt;T&gt;](./pipefiltercore.ipipeandfilteraggregate-1.md)

### <a id="methods-withgotocondition"/>**WithGotoCondition(Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;, String, String)**

Add new go to condition to the aggregate pipe.
 <br>If the condition is true, jump to the given pipe without executing the current pipe.<br>If the false condition continues.

```csharp
IPipeAndFilterAggregate<T> WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<Boolean>> condition, string aliasgoto, string namecondition)
```

#### Parameters

`condition` Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;<br>
The handle to execute.

`aliasgoto` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias to another pipe.

`namecondition` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for condition(optional).

#### Returns

[IPipeAndFilterAggregate&lt;T&gt;](./pipefiltercore.ipipeandfilteraggregate-1.md)


- - -
[**Back to List Api**](./apis.md)
