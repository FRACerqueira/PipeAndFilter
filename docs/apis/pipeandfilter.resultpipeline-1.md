# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:ResultPipeline<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# ResultPipeline&lt;T&gt;

Namespace: PipeAndFilter

Represents the result of pipeline

```csharp
public struct ResultPipeline<T>
```

#### Type Parameters

`T`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [ResultPipeline&lt;T&gt;](./pipeandfilter.resultpipeline-1.md)

## Properties

### <a id="properties-aborted"/>**Aborted**

Pipeline is aborted.

```csharp
public bool Aborted { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-pipeexception"/>**PipeException**

The last exception in step (pipe,condition or task).

```csharp
public Exception PipeException { get; }
```

#### Property Value

[Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>

### <a id="properties-status"/>**Status**

Status details of all pipes

```csharp
public ImmutableArray<PipeRanStatus> Status { get; }
```

#### Property Value

ImmutableArray&lt;PipeRanStatus&gt;<br>

### <a id="properties-value"/>**Value**

Contract contract pipeline

```csharp
public T Value { get; }
```

#### Property Value

T<br>

## Constructors

### <a id="constructors-.ctor"/>**ResultPipeline()**

Create ResultPipeline

```csharp
ResultPipeline()
```

#### Exceptions

[PipeAndFilterException](./pipeandfilter.pipeandfilterexception.md)<br>
Message error

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**ResultPipeline(T, Boolean, Exception, ImmutableArray&lt;PipeRanStatus&gt;)**

Create ResultPipeline

```csharp
ResultPipeline(T value, bool aborted, Exception exception, ImmutableArray<PipeRanStatus> status)
```

#### Parameters

`value` T<br>
The contract value

`aborted` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Pipeline aborted

`exception` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
The last exception

`status` ImmutableArray&lt;PipeRanStatus&gt;<br>
Detailed running status of all pipes


- - -
[**Back to List Api**](./apis.md)
