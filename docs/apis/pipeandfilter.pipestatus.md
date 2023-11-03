# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:PipeStatus 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# PipeStatus

Namespace: PipeAndFilter

Represents the status of step (pipe, condition or task)

```csharp
public struct PipeStatus
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [PipeStatus](./pipeandfilter.pipestatus.md)

## Properties

### <a id="properties-alias"/>**Alias**

Get execution alias

```csharp
public string Alias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-condition"/>**Condition**

Get condition result

```csharp
public bool Condition { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-elapsedtime"/>**Elapsedtime**

Get the elapsed time

```csharp
public TimeSpan Elapsedtime { get; }
```

#### Property Value

[TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

### <a id="properties-gotoalias"/>**GotoAlias**

Get go to Alias

```csharp
public string GotoAlias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-typeexec"/>**TypeExec**

Get Type execution

```csharp
public HandlerType TypeExec { get; }
```

#### Property Value

[HandlerType](./pipeandfilter.handlertype.md)<br>

### <a id="properties-value"/>**Value**

Get status execution

```csharp
public TaskStatus Value { get; }
```

#### Property Value

[TaskStatus](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskstatus)<br>

## Constructors

### <a id="constructors-.ctor"/>**PipeStatus()**

Create PipeStatus

```csharp
PipeStatus()
```

#### Exceptions

[PipeAndFilterException](./pipeandfilter.pipeandfilterexception.md)<br>
Message error

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**PipeStatus(HandlerType, TaskStatus, TimeSpan, String, String, Boolean)**

Create instance

```csharp
PipeStatus(HandlerType typeExec, TaskStatus value, TimeSpan elapsedtime, string alias, string gotoAlias, bool condition)
```

#### Parameters

`typeExec` [HandlerType](./pipeandfilter.handlertype.md)<br>
[HandlerType](./pipeandfilter.handlertype.md)

`value` [TaskStatus](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskstatus)<br>
[TaskStatus](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskstatus)

`elapsedtime` [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>
[TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)

`alias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias

`gotoAlias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
the go to alias

`condition` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if Status is condition handle


- - -
[**Back to List Api**](./apis.md)
