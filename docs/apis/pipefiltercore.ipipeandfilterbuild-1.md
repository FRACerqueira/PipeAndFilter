# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterBuild<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterBuild&lt;T&gt;

Namespace: PipeFilterCore

Represents the commands for build a service and Create the instance to run.

```csharp
public interface IPipeAndFilterBuild<T>
```

#### Type Parameters

`T`<br>
Type of contract.

## Methods

### <a id="methods-build"/>**Build(String)**

Build PipeAndFilter to add into ServiceCollection.

```csharp
IPipeAndFilterService<T> Build(string serviceId)
```

#### Parameters

`serviceId` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The service Id.

#### Returns

[IPipeAndFilterService&lt;T&gt;](./pipefiltercore.ipipeandfilterservice-1.md)

### <a id="methods-buildandcreate"/>**BuildAndCreate()**

Build and create PipeAndFilter to run.

```csharp
IPipeAndFilterInit<T> BuildAndCreate()
```

#### Returns

[IPipeAndFilterInit&lt;T&gt;](./pipefiltercore.ipipeandfilterinit-1.md)


- - -
[**Back to List Api**](./apis.md)
