# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterInit<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterInit&lt;T&gt;

Namespace: PipeFilterPlus

Represents command for initialization

```csharp
public interface IPipeAndFilterInit<T>
```

#### Type Parameters

`T`<br>
Type of contract

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

[IPipeAndFilter&lt;T&gt;](./pipefilterplus.ipipeandfilter-1.md)

**Remarks:**

Alias ​​is used to reference in another pipe.

### <a id="methods-correlationid"/>**CorrelationId(String)**

The Correlation Id

```csharp
IPipeAndFilterInit<T> CorrelationId(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Correlation Id value

#### Returns

[IPipeAndFilterInit&lt;T&gt;](./pipefilterplus.ipipeandfilterinit-1.md)

### <a id="methods-init"/>**Init(T)**

Initial contract value.

```csharp
IPipeAndFilterInit<T> Init(T contract)
```

#### Parameters

`contract` T<br>
The contract.

#### Returns

[IPipeAndFilterInit&lt;T&gt;](./pipefilterplus.ipipeandfilterinit-1.md)

### <a id="methods-logger"/>**Logger(ILogger)**

The logger handler

```csharp
IPipeAndFilterInit<T> Logger(ILogger value)
```

#### Parameters

`value` ILogger<br>
logger handler value

#### Returns

[IPipeAndFilterInit&lt;T&gt;](./pipefilterplus.ipipeandfilterinit-1.md)

### <a id="methods-maxdegreeprocess"/>**MaxDegreeProcess(Int32)**

Maximum number of concurrent tasks enable.

```csharp
IPipeAndFilterInit<T> MaxDegreeProcess(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of concurrent tasks.

#### Returns

[IPipeAndFilterInit&lt;T&gt;](./pipefilterplus.ipipeandfilterinit-1.md)

**Remarks:**

The default value is number of processors.


- - -
[**Back to List Api**](./apis.md)
