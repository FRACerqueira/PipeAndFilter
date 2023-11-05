# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilter<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilter&lt;T&gt;

Namespace: PipeFilterPlus

Represents the commands for pipe.

```csharp
public interface IPipeAndFilter<T>
```

#### Type Parameters

`T`<br>
Type of contract.

## Methods

### <a id="methods-addpipe"/>**AddPipe(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new pipe.

```csharp
IPipeAndFilter<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler pipe to execute.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe.
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).

#### Returns

[IPipeAndFilter&lt;T&gt;](./pipefilterplus.ipipeandfilter-1.md)

**Remarks:**

Alias ​​is used to reference in another pipe.

### <a id="methods-addpipetasks"/>**AddPipeTasks(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new pipe aggregate tasks.

```csharp
IPipeAndFilterTasks<T> AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler pipe aggregate to execute.
 <br>The handler command will run after all tasks are executed.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe.
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).

#### Returns

[IPipeAndFilterTasks&lt;T&gt;](./pipefilterplus.ipipeandfiltertasks-1.md)

**Remarks:**

Alias ​​is used to reference in another pipe.

### <a id="methods-run"/>**Run()**

Execute PipeAndFilter.

```csharp
ValueTask<ResultPipeAndFilter<T>> Run()
```

#### Returns

[ResultPipeAndFilter&lt;T&gt;](./pipefilterplus.resultpipeandfilter-1.md)

### <a id="methods-withcondition"/>**WithCondition(Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;, String, String)**

Add new condition.

```csharp
IPipeAndFilterConditions<T> WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<Boolean>> condition, string aliasgoto, string namecondition)
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

[IPipeAndFilterConditions&lt;T&gt;](./pipefilterplus.ipipeandfilterconditions-1.md)


- - -
[**Back to List Api**](./apis.md)
