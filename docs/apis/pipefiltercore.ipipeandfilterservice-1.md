# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterService<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterService&lt;T&gt;

Namespace: PipeFilterCore

Represents the commands for create a instance.

```csharp
public interface IPipeAndFilterService<T>
```

#### Type Parameters

`T`<br>
Type of contract.

## Properties

### <a id="properties-serviceid"/>**ServiceId**

The service id for this type.

```csharp
public abstract string ServiceId { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Methods

### <a id="methods-create"/>**Create()**

Create a instance.

```csharp
IPipeAndFilterInit<T> Create()
```

#### Returns

[IPipeAndFilterInit&lt;T&gt;](./pipefiltercore.ipipeandfilterinit-1.md)


- - -
[**Back to List Api**](./apis.md)
