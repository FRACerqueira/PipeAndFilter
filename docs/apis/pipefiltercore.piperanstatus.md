# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:PipeRanStatus 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# PipeRanStatus

Namespace: PipeFilterCore

Represents the ran status of the pipe.

```csharp
public struct PipeRanStatus
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [PipeRanStatus](./pipefiltercore.piperanstatus.md)

## Properties

### <a id="properties-alias"/>**Alias**

The pipe alias.

```csharp
public string Alias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-count"/>**Count**

The number of times the pipe has been executed.

```csharp
public int Count { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-status"/>**Status**

The last execution status of the pipe.

```csharp
public PipeStatus Status { get; }
```

#### Property Value

[PipeStatus](./pipefiltercore.pipestatus.md)<br>

### <a id="properties-statusdetails"/>**StatusDetails**

The detailed status of each step (pipe, conditions and tasks).

```csharp
public ImmutableArray<PipeStatus> StatusDetails { get; }
```

#### Property Value

ImmutableArray&lt;PipeStatus&gt;<br>

## Constructors

### <a id="constructors-.ctor"/>**PipeRanStatus()**

Create PipeRanStatus.
 <br>Do not use this constructor!

```csharp
PipeRanStatus()
```

#### Exceptions

[PipeAndFilterException](./pipefiltercore.pipeandfilterexception.md)<br>
Message error

### <a id="constructors-.ctor"/>**PipeRanStatus(String, String, ImmutableArray&lt;PipeStatus&gt;)**

Create PipeRanStatus (Only internal use or Unit-Test).

```csharp
PipeRanStatus(string id, string alias, ImmutableArray<PipeStatus> details)
```

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The pipe Id.

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The pipe alias.

`details` ImmutableArray&lt;PipeStatus&gt;<br>
The detailed status of all runs.


- - -
[**Back to List Api**](./apis.md)
