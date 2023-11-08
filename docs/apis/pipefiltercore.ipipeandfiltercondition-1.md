# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterCondition<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterCondition&lt;T&gt;

Namespace: PipeFilterCore

Represents commands for conditions.

```csharp
public interface IPipeAndFilterCondition<T> : IPipeAndFilterBuild<T>
```

#### Type Parameters

`T`<br>
Type of contract.

Implements IPipeAndFilterBuild&lt;T&gt;

## Methods

### <a id="methods-addpipe"/>**AddPipe(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new pipe.

```csharp
IPipeAndFilterAdd<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler pipe to execute.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe.
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).<br>Alias ​​is used to reference in another pipe.

#### Returns

[IPipeAndFilterAdd&lt;T&gt;](./pipefiltercore.ipipeandfilteradd-1.md)

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
 <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).<br>Alias ​​is used to reference in another pipe.

#### Returns

[IPipeAndFilterTasks&lt;T&gt;](./pipefiltercore.ipipeandfiltertasks-1.md)

### <a id="methods-withcondition"/>**WithCondition(Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;, String)**

Add new condition.

```csharp
IPipeAndFilterCondition<T> WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<Boolean>> condition, string namecondition)
```

#### Parameters

`condition` Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;<br>
The handle condition to execute.

`namecondition` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for condition(optional).

#### Returns

[IPipeAndFilterCondition&lt;T&gt;](./pipefiltercore.ipipeandfiltercondition-1.md)

### <a id="methods-withgotocondition"/>**WithGotoCondition(Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;, String, String)**

Add new go to condition.
 <br>If the condition is true, jump to the given pipe without executing the current pipe.<br>If the false condition continues.

```csharp
IPipeAndFilterCondition<T> WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<Boolean>> condition, string aliasgoto, string namecondition)
```

#### Parameters

`condition` Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;<br>
The handle condition to execute.

`aliasgoto` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias to another pipe.

`namecondition` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for condition(optional).

#### Returns

[IPipeAndFilterCondition&lt;T&gt;](./pipefiltercore.ipipeandfiltercondition-1.md)


- - -
[**Back to List Api**](./apis.md)
