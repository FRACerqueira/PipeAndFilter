# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterRun<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterRun&lt;T&gt;

Namespace: PipeFilterCore

Represents the command for Run.

```csharp
public interface IPipeAndFilterRun<T>
```

#### Type Parameters

`T`<br>
Type of contract.

## Methods

### <a id="methods-run"/>**Run(Nullable&lt;CancellationToken&gt;)**

Execute PipeAndFilter.

```csharp
ValueTask<ResultPipeAndFilter<T>> Run(Nullable<CancellationToken> cancellation)
```

#### Parameters

`cancellation` [Nullable&lt;CancellationToken&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

#### Returns

[ResultPipeAndFilter&lt;T&gt;](./pipefiltercore.resultpipeandfilter-1.md)


- - -
[**Back to List Api**](./apis.md)
