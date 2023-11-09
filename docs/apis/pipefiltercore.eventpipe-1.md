# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:EventPipe<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# EventPipe&lt;T&gt;

Namespace: PipeFilterCore

Represents a pipe/task event with parameters, values ​​and commands.

```csharp
public class EventPipe<T>
```

#### Type Parameters

`T`<br>
Type of contract.

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventPipe&lt;T&gt;](./pipefiltercore.eventpipe-1.md)

## Properties

### <a id="properties-correlationid"/>**CorrelationId**

The Correlation Id.

```csharp
public string CorrelationId { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-currentalias"/>**CurrentAlias**

The current Alias.

```csharp
public string CurrentAlias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-fromalias"/>**FromAlias**

The previous Alias.

```csharp
public string FromAlias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-logger"/>**Logger**

The log handler.

```csharp
public ILogger Logger { get; }
```

#### Property Value

ILogger<br>

## Constructors

### <a id="constructors-.ctor"/>**EventPipe(String, ILogger, Action&lt;Action&lt;T&gt;&gt;, ImmutableDictionary&lt;String, String&gt;, String, String, String, String)**

Create instance of Event-Pipe (Only internal use or Unit-Test).

```csharp
public EventPipe(string cid, ILogger logger, Action<Action<T>> changecontract, ImmutableDictionary<String, String> savedvalues, string fromId, string currentId, string fromAlias, string currentAlias)
```

#### Parameters

`cid` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The correlation Id.

`logger` ILogger<br>
Handle of log.

`changecontract` Action&lt;Action&lt;T&gt;&gt;<br>
Handle of changecontract.

`savedvalues` ImmutableDictionary&lt;String, String&gt;<br>
The values saved.

`fromId` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The previous Id.

`currentId` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The current Id.

`fromAlias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The previous alias.

`currentAlias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The current alias.

## Methods

### <a id="methods-endpipeandfilter"/>**EndPipeAndFilter()**

End PipeAndFilter.

```csharp
public void EndPipeAndFilter()
```

### <a id="methods-removalueatend"/>**RemoValueAtEnd(String)**

Remove a value associated with a unique key at the end of this event (If this event is not a task event).
 <br>Values ​​removed in the task event will only take effect in the pipe aggregation event<br>A task event cannot see values ​​saved and/or removed by another task.<br>In a task event, Never try to overwrite a value already saved by another event, the results may not be as expected as the execution sequence is not guaranteed.

```csharp
public void RemoValueAtEnd(string id)
```

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique key Id.

### <a id="methods-savevalueatend"/>**SaveValueAtEnd&lt;T1&gt;(String, T1)**

Save/replace a value associated with a unique key at the end of this event(If this event is not a task event).
 <br>Values ​​saved in the task event will only take effect in the pipe aggregation event<br>A task event cannot see values ​​saved and/or removed by another task.<br>In a task event, Never try to overwrite a value already saved by another event, the results may not be as expected as the execution sequence is not guaranteed.<br>The values ​​will serialize into json.

```csharp
public void SaveValueAtEnd<T1>(string id, T1 value)
```

#### Type Parameters

`T1`<br>
Type value to save.

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique key Id.

`value` T1<br>
The value to save.

### <a id="methods-threadsafeaccess"/>**ThreadSafeAccess(Action&lt;T&gt;)**

Thread Safe Access of contract.
 <br>The action will only be executed if the contract exists(not null).

```csharp
public void ThreadSafeAccess(Action<T> action)
```

#### Parameters

`action` Action&lt;T&gt;<br>
The action to access.
 <br>The action will only be executed if the contract exists(not null).

### <a id="methods-trysavedvalue"/>**TrySavedValue(String, ref String)**

Try get value saved ​​associated with a unique key.
 <br>The values ​​are serialized in json.<br>Null result may exist.

```csharp
public bool TrySavedValue(string id, ref String value)
```

#### Parameters

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique key Id.

`value` [String&](https://docs.microsoft.com/en-us/dotnet/api/system.string&)<br>
The value saved if any.

#### Returns

True if value exists, otherwise false with null value


- - -
[**Back to List Api**](./apis.md)
