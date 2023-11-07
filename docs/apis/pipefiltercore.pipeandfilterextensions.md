# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:PipeAndFilterExtensions 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# PipeAndFilterExtensions

Namespace: PipeFilterCore

Represents the extensions to add PipeAndFilter to the ServiceCollection.

```csharp
public static class PipeAndFilterExtensions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [PipeAndFilterExtensions](./pipefiltercore.pipeandfilterextensions.md)

## Methods

### <a id="methods-addpipeandfilter"/>**AddPipeAndFilter&lt;T&gt;(IServiceCollection, IPipeAndFilterServiceBuild&lt;T&gt;)**

Add PipeAndFilter to ServiceCollection.

```csharp
public static IServiceCollection AddPipeAndFilter<T>(IServiceCollection services, IPipeAndFilterServiceBuild<T> pipeAndFilterServiceBuild)
```

#### Type Parameters

`T`<br>
Type of contract.

#### Parameters

`services` IServiceCollection<br>
The .

`pipeAndFilterServiceBuild` IPipeAndFilterServiceBuild&lt;T&gt;<br>
The PipeAndFilter

#### Returns




- - -
[**Back to List Api**](./apis.md)
