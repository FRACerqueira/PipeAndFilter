# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterStart<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterStart&lt;T&gt;

Namespace: PipeFilterCore

Represents the commands for Pipes.

```csharp
public interface IPipeAndFilterStart<T> : IPipeAndFilterBuild<T>
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


- - -
[**Back to List Api**](./apis.md)
