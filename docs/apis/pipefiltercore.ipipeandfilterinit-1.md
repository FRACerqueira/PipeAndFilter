# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterInit<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterInit&lt;T&gt;

Namespace: PipeFilterCore

Represents commands for initialization and run.

```csharp
public interface IPipeAndFilterInit<T> : IPipeAndFilterRun<T>
```

#### Type Parameters

`T`<br>
Type of contract.

Implements IPipeAndFilterRun&lt;T&gt;

## Properties

### <a id="properties-serviceid"/>**ServiceId**

The service id for this type.

```csharp
public abstract string ServiceId { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Methods

### <a id="methods-correlationid"/>**CorrelationId(String)**

The Correlation Id.

```csharp
IPipeAndFilterInit<T> CorrelationId(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Correlation Id value.

#### Returns

[IPipeAndFilterInit&lt;T&gt;](./pipefiltercore.ipipeandfilterinit-1.md)

### <a id="methods-init"/>**Init(T)**

Initial contract value.

```csharp
IPipeAndFilterInit<T> Init(T contract)
```

#### Parameters

`contract` T<br>
The contract.

#### Returns

[IPipeAndFilterInit&lt;T&gt;](./pipefiltercore.ipipeandfilterinit-1.md)

### <a id="methods-logger"/>**Logger(ILogger)**

The logger handler.

```csharp
IPipeAndFilterInit<T> Logger(ILogger value)
```

#### Parameters

`value` ILogger<br>
logger handler value.

#### Returns

[IPipeAndFilterInit&lt;T&gt;](./pipefiltercore.ipipeandfilterinit-1.md)


- - -
[**Back to List Api**](./apis.md)
