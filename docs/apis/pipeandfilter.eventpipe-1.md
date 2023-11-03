# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:EventPipe<T> 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# EventPipe&lt;T&gt;

Namespace: PipeAndFilter

Represents a pipe/task event with parameters, values ​​and commands

```csharp
public class EventPipe<T>
```

#### Type Parameters

`T`<br>
Type of contract

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventPipe&lt;T&gt;](./pipeandfilter.eventpipe-1.md)

## Properties

### <a id="properties-currentalias"/>**CurrentAlias**

Get current Alias

```csharp
public string CurrentAlias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-currentid"/>**CurrentId**

Get current Id

```csharp
public string CurrentId { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-fromalias"/>**FromAlias**

Get from Alias

```csharp
public string FromAlias { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-fromid"/>**FromId**

Get from Id

```csharp
public string FromId { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-savedpipes"/>**SavedPipes**

Get values saved ​​associated with pipes
 <br>The values ​​are serialized in json<br>Null result may exist

```csharp
public ImmutableArray<ValueTuple<String, String, String>> SavedPipes { get; }
```

#### Property Value

ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;<br>

### <a id="properties-savedtasks"/>**SavedTasks**

Get values saved ​​associated with tasks
 <br>The values ​​are serialized in json<br>Null result may exist

```csharp
public ImmutableArray<ValueTuple<String, String, String>> SavedTasks { get; }
```

#### Property Value

ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;<br>

**Remarks:**

Data only exists when executed by an aggregator pipe

## Constructors

### <a id="constructors-.ctor"/>**EventPipe(Action&lt;Action&lt;T&gt;&gt;, ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;, ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;, String, String, String, String)**

Create instance of EventPipe (Only internal use or Unit-Test)

```csharp
public EventPipe(Action<Action<T>> changecontract, ImmutableArray<ValueTuple<String, String, String>> savedpipes, ImmutableArray<ValueTuple<String, String, String>> savedtasks, string fromId, string currentId, string fromAlias, string currentAlias)
```

#### Parameters

`changecontract` Action&lt;Action&lt;T&gt;&gt;<br>
Handle of changecontract control

`savedpipes` ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;<br>
The values saved by pipe

`savedtasks` ImmutableArray&lt;ValueTuple&lt;String, String, String&gt;&gt;<br>
The values saved by tasks

`fromId` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The came from Id

`currentId` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The current Id

`fromAlias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias came from Id

`currentAlias` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The alias current Id

## Methods

### <a id="methods-changecontract"/>**ChangeContract(Action&lt;T&gt;)**

Change value contract

```csharp
public void ChangeContract(Action<T> action)
```

#### Parameters

`action` Action&lt;T&gt;<br>

**Remarks:**

The action will only be executed if the contract exists.
 <br>

### <a id="methods-endpipeline"/>**EndPipeline()**

End EndPipeline control

```csharp
public void EndPipeline()
```

### <a id="methods-removesavedvalue"/>**RemoveSavedValue()**

Remove a value associated with this pipe or task

```csharp
public void RemoveSavedValue()
```

### <a id="methods-savevalue"/>**SaveValue&lt;T1&gt;(T1)**

Save/overwrite a value associated with this pipe or task

```csharp
public void SaveValue<T1>(T1 value)
```

#### Type Parameters

`T1`<br>
Type value to save

#### Parameters

`value` T1<br>
The value to save

**Remarks:**

The values ​​will serialize into json


- - -
[**Back to List Api**](./apis.md)
