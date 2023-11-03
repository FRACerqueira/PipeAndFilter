# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipelineInit<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipelineInit&lt;T&gt;

Namespace: PipeAndFilter

Represents Pipeline start config

```csharp
public interface IPipelineInit<T>
```

#### Type Parameters

`T`<br>
Type of contract

## Methods

### <a id="methods-addpipe"/>**AddPipe(Func&lt;EventPipe&lt;T&gt;, CancellationToken, Task&gt;, String)**

Add the pipe

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

### <a id="methods-init"/>**Init(T)**

Initial contract value

```csharp
IPipelineInit<T> Init(T contract)
```

#### Parameters

`contract` T<br>
The contract

#### Returns

[IPipelineInit&lt;T&gt;](./pipeandfilter.ipipelineinit-1.md)

### <a id="methods-maxdegreeprocess"/>**MaxDegreeProcess(Int32)**

Maximum number of concurrent tasks enable. Default vaue is number of processors.

```csharp
IPipelineInit<T> MaxDegreeProcess(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of concurrent tasks

#### Returns

[IPipelineInit&lt;T&gt;](./pipeandfilter.ipipelineinit-1.md)


- - -
[**Back to List Api**](./apis.md)
