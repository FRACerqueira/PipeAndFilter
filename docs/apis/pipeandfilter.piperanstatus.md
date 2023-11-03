# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:PipeRanStatus 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# PipeRanStatus

Namespace: PipeAndFilter

Represents the ran status of the pipe

```csharp
public struct PipeRanStatus
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [PipeRanStatus](./pipeandfilter.piperanstatus.md)

## Properties

### <a id="properties-alias"/>**Alias**

Get alias pipe

```csharp
public string Alias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-count"/>**Count**

Get number of times the pipe was executed

```csharp
public int Count { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-id"/>**Id**

Get pipe Id

```csharp
public string Id { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-status"/>**Status**

Get last pipe status handle run

```csharp
public PipeStatus Status { get; }
```

#### Property Value

[PipeStatus](./pipeandfilter.pipestatus.md)<br>

### <a id="properties-statusdetails"/>**StatusDetails**

Get detailed status for each step(pipe,conditions and tasks)

```csharp
public IEnumerable<PipeStatus> StatusDetails { get; }
```

#### Property Value

[IEnumerable&lt;PipeStatus&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>

## Constructors

### <a id="constructors-.ctor"/>**PipeRanStatus()**

Create PipeRanStatus

```csharp
PipeRanStatus()
```

#### Exceptions

[PipeAndFilterException](./pipeandfilter.pipeandfilterexception.md)<br>
Message error

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**PipeRanStatus(String, String, IEnumerable&lt;PipeStatus&gt;)**

Create PipeRanStatus

```csharp
PipeRanStatus(string id, string alias, IEnumerable<PipeStatus> details)
```

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The id pipe

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias pipe

`details` [IEnumerable&lt;PipeStatus&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
The detailed status of all runs


- - -
[**Back to List Api**](./apis.md)
