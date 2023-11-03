# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:Pipeline 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# Pipeline

Namespace: PipeAndFilter

Represents PipeAndFilter Extension

```csharp
public static class Pipeline
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Pipeline](./pipeandfilter.pipeline.md)

## Methods

### <a id="methods-create"/>**Create&lt;T&gt;(Nullable&lt;CancellationToken&gt;)**

Create Pipeline control

```csharp
public static IPipelineInit<T> Create<T>(Nullable<CancellationToken> cts)
```

#### Type Parameters

`T`<br>
Type of return

#### Parameters

`cts` [Nullable&lt;CancellationToken&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
[CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken)

#### Returns

[IPipelineInit&lt;T&gt;](./pipeandfilter.ipipelineinit-1.md)


- - -
[**Back to List Api**](./apis.md)
