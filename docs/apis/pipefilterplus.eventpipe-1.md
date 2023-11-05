# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:EventPipe<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# EventPipe&lt;T&gt;

Namespace: PipeFilterPlus

Represents a pipe/task event with parameters, values ​​and commands.

```csharp
public class EventPipe<T>
```

#### Type Parameters

`T`<br>
Type of contract.

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventPipe&lt;T&gt;](./pipefilterplus.eventpipe-1.md)

## Properties

### <a id="properties-correlationid"/>**CorrelationId**

The Correlation Id

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

The log handler

```csharp
public ILogger Logger { get; }
```

#### Property Value

ILogger<br>

### <a id="properties-savedpipes"/>**SavedPipes**

The values saved ​​associated with pipes

```csharp
public ImmutableArray<ValueTuple<String, String, String>> SavedPipes { get; }
```

#### Property Value

ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;<br>

**Remarks:**

The values ​​are serialized in json.
 <br>Null result may exist.

### <a id="properties-savedtasks"/>**SavedTasks**

The values saved ​​associated with tasks.

```csharp
public ImmutableArray<ValueTuple<String, String, String>> SavedTasks { get; }
```

#### Property Value

ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;<br>

**Remarks:**

Data only exists when executed by an aggregator pipe.
 <br>The values ​​are serialized in json.<br>Null result may exist.

## Constructors

### <a id="constructors-.ctor"/>**EventPipe(String, ILogger, Action&lt;Action&lt;T&gt;&gt;, ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;, ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;, String, String, String, String)**

Create instance of Event-Pipe (Only internal use or Unit-Test).

```csharp
public EventPipe(string cid, ILogger logger, Action<Action<T>> changecontract, ImmutableArray<ValueTuple<String, String, String>> savedpipes, ImmutableArray<ValueTuple<String, String, String>> savedtasks, string fromId, string currentId, string fromAlias, string currentAlias)
```

#### Parameters

`cid` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The correlation Id.

`logger` ILogger<br>
Handle of log.

`changecontract` Action&lt;Action&lt;T&gt;&gt;<br>
Handle of changecontract.

`savedpipes` ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;<br>
The values saved by pipe.

`savedtasks` ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;<br>
The values saved by tasks.

`fromId` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The previous Id.

`currentId` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The current Id.

`fromAlias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The previous alias.

`currentAlias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The current alias.

## Methods

### <a id="methods-changecontract"/>**ChangeContract(Action&lt;T&gt;)**

Change value contract.

```csharp
public void ChangeContract(Action<T> action)
```

#### Parameters

`action` Action&lt;T&gt;<br>
The action to change value.

**Remarks:**

The action will only be executed if the contract exists.
 <br>See .

### <a id="methods-endpipeandfilter"/>**EndPipeAndFilter()**

End PipeAndFilter.

```csharp
public void EndPipeAndFilter()
```

### <a id="methods-removesavedvalue"/>**RemoveSavedValue()**

Remove a value associated with this pipe or task .

```csharp
public void RemoveSavedValue()
```

### <a id="methods-savevalue"/>**SaveValue&lt;T1&gt;(T1)**

Save/overwrite a value associated with this pipe or task.

```csharp
public void SaveValue<T1>(T1 value)
```

#### Type Parameters

`T1`<br>
Type value to save.

#### Parameters

`value` T1<br>
The value to save.

**Remarks:**

The values ​​will serialize into json.


- - -
[**Back to List Api**](./apis.md)
