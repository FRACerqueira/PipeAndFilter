# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:PipeStatus 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# PipeStatus

Namespace: PipeFilterCore

Represents the status of step (pipe, condition or task).

```csharp
public struct PipeStatus
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [PipeStatus](./pipefiltercore.pipestatus.md)

## Properties

### <a id="properties-alias"/>**Alias**

The alias execution.

```csharp
public string Alias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-condition"/>**Condition**

The result of the condition.

```csharp
public bool Condition { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-elapsedtime"/>**Elapsedtime**

The elapsed time.

```csharp
public TimeSpan Elapsedtime { get; }
```

#### Property Value

[TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

### <a id="properties-gotoalias"/>**GotoAlias**

The Alias ​​link.

```csharp
public string GotoAlias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-typeexec"/>**TypeExec**

The Type handle.

```csharp
public HandlerType TypeExec { get; }
```

#### Property Value

[HandlerType](./pipefiltercore.handlertype.md)<br>

### <a id="properties-value"/>**Value**

The running status.

```csharp
public TaskStatus Value { get; }
```

#### Property Value

[TaskStatus](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskstatus)<br>

## Constructors

### <a id="constructors-.ctor"/>**PipeStatus()**

Create Pipe Status.

```csharp
PipeStatus()
```

#### Exceptions

[PipeAndFilterException](./pipefiltercore.pipeandfilterexception.md)<br>
Message error.

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**PipeStatus(HandlerType, TaskStatus, TimeSpan, String, String, Boolean)**

Create instance (Only internal use or Unit-Test).

```csharp
PipeStatus(HandlerType typeExec, TaskStatus value, TimeSpan elapsedtime, string alias, string gotoAlias, bool condition)
```

#### Parameters

`typeExec` [HandlerType](./pipefiltercore.handlertype.md)<br>
Type handle. See [HandlerType](./pipefiltercore.handlertype.md).

`value` [TaskStatus](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskstatus)<br>
The Status. See [TaskStatus](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskstatus).

`elapsedtime` [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>
The elapsed time. See [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan).

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias execution.

`gotoAlias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias link.

`condition` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
The result of the condition for execution.


- - -
[**Back to List Api**](./apis.md)
