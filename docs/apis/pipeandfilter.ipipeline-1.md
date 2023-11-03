# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeline<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeline&lt;T&gt;

Namespace: PipeAndFilter

Represents Pipeline commands

```csharp
public interface IPipeline<T>
```

#### Type Parameters

`T`<br>
Type of contract

## Methods

### <a id="methods-addpipe"/>**AddPipe(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new pipe

```csharp
IPipeline<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler pipe to execute

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe (optional).
 <br>Tip: Alias is used for go to operation

#### Returns

[IPipeline&lt;T&gt;](./pipeandfilter.ipipeline-1.md)

### <a id="methods-addpipetasks"/>**AddPipeTasks(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add new pipe aggregate tasks.
 <br>The handler command will run after all tasks are executed.

```csharp
IPipelineTasks<T> AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string alias)
```

#### Parameters

`command` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;<br>
The handler pipe aggregate to execute

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique alias for pipe (optional).
 <br>Tip: Alias is used for go to operation

#### Returns

[IPipeline&lt;T&gt;](./pipeandfilter.ipipeline-1.md)

### <a id="methods-run"/>**Run()**

Execute the pipeline

```csharp
ValueTask<ResultPipeline<T>> Run()
```

#### Returns

[ResultPipeline&lt;T&gt;](./pipeandfilter.resultpipeline-1.md)

### <a id="methods-withcondition"/>**WithCondition(Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;, String, String)**

Add new condition for run this pipe

```csharp
IPipelineConditions<T> WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<Boolean>> condition, string aliasgoto, string namecondition)
```

#### Parameters

`condition` Func&lt;EventPipe&lt;T&gt;, CancellationToken, ValueTask&lt;Boolean&gt;&gt;<br>
The handle condition to execute

`aliasgoto` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias go to handle.
 <br>Tip: If condition not have go to, the value must be null

`namecondition` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The name for condition(optional).

#### Returns

[IPipelineConditions&lt;T&gt;](./pipeandfilter.ipipelineconditions-1.md)


- - -
[**Back to List Api**](./apis.md)
