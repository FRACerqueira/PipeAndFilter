# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:IPipeAndFilterRunService<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# IPipeAndFilterRunService&lt;T&gt;

Namespace: PipeFilterCore

Represents commands for initialization and run.

```csharp
public interface IPipeAndFilterRunService<T> : IPipeAndFilterRun<T>
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
IPipeAndFilterRunService<T> CorrelationId(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Correlation Id value.

#### Returns

[IPipeAndFilterRunService&lt;T&gt;](./pipefiltercore.ipipeandfilterrunservice-1.md)

### <a id="methods-init"/>**Init(T)**

Initial contract value.

```csharp
IPipeAndFilterRunService<T> Init(T contract)
```

#### Parameters

`contract` T<br>
The contract.

#### Returns

[IPipeAndFilterRunService&lt;T&gt;](./pipefiltercore.ipipeandfilterrunservice-1.md)

### <a id="methods-logger"/>**Logger(ILogger)**

The logger handler.

```csharp
IPipeAndFilterRunService<T> Logger(ILogger value)
```

#### Parameters

`value` ILogger<br>
logger handler value.

#### Returns

[IPipeAndFilterRunService&lt;T&gt;](./pipefiltercore.ipipeandfilterrunservice-1.md)


- - -
[**Back to List Api**](./apis.md)
