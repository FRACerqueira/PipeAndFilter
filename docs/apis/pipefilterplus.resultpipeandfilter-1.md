# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:ResultPipeAndFilter<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# ResultPipeAndFilter&lt;T&gt;

Namespace: PipeFilterPlus

Represents the result of PipeAndFilter

```csharp
public struct ResultPipeAndFilter<T>
```

#### Type Parameters

`T`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [ResultPipeAndFilter&lt;T&gt;](./pipefilterplus.resultpipeandfilter-1.md)

## Properties

### <a id="properties-aborted"/>**Aborted**

If aborted.

```csharp
public bool Aborted { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-exception"/>**Exception**

The exception (pipe,condition or task), if any.

```csharp
public PipeAndFilterException Exception { get; }
```

#### Property Value

[PipeAndFilterException](./pipefilterplus.pipeandfilterexception.md)<br>

### <a id="properties-status"/>**Status**

The status details of all pipes

```csharp
public ImmutableArray<PipeRanStatus> Status { get; }
```

#### Property Value

ImmutableArray&lt;PipeRanStatus&gt;<br>

### <a id="properties-value"/>**Value**

The Contract value

```csharp
public T Value { get; }
```

#### Property Value

T<br>

## Constructors

### <a id="constructors-.ctor"/>**ResultPipeAndFilter()**

Create Result of PipeAndFilter

```csharp
ResultPipeAndFilter()
```

#### Exceptions

[PipeAndFilterException](./pipefilterplus.pipeandfilterexception.md)<br>
Message error

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**ResultPipeAndFilter(T, Boolean, PipeAndFilterException, ImmutableArray&lt;PipeRanStatus&gt;)**

Create Result of PipeAndFilter (Only internal use or Unit-Test).

```csharp
ResultPipeAndFilter(T value, bool aborted, PipeAndFilterException exception, ImmutableArray<PipeRanStatus> status)
```

#### Parameters

`value` T<br>
The contract value.

`aborted` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
If aborted.

`exception` [PipeAndFilterException](./pipefilterplus.pipeandfilterexception.md)<br>
The exception, if any.

`status` ImmutableArray&lt;PipeRanStatus&gt;<br>
Detailed running status of all pipes.


- - -
[**Back to List Api**](./apis.md)
