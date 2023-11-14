# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:PipeAndFilterException 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# PipeAndFilterException

Namespace: PipeFilterCore

Represents a exception for PipeAndFilter.

```csharp
public class PipeAndFilterException : System.Exception, System.Runtime.Serialization.ISerializable
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception) → [PipeAndFilterException](./pipefiltercore.pipeandfilterexception.md)<br>
Implements [ISerializable](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iserializable)

## Fields

### <a id="fields-statusinit"/>**StatusInit**

The default status for initialize.

```csharp
public static PipeStatus StatusInit;
```

## Properties

### <a id="properties-data"/>**Data**

```csharp
public IDictionary Data { get; }
```

#### Property Value

[IDictionary](https://docs.microsoft.com/en-us/dotnet/api/system.collections.idictionary)<br>

### <a id="properties-helplink"/>**HelpLink**

```csharp
public string HelpLink { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-hresult"/>**HResult**

```csharp
public int HResult { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-innerexception"/>**InnerException**

```csharp
public Exception InnerException { get; }
```

#### Property Value

[Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>

### <a id="properties-message"/>**Message**

```csharp
public string Message { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-source"/>**Source**

```csharp
public string Source { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-stacktrace"/>**StackTrace**

```csharp
public string StackTrace { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-status"/>**Status**

The status of step (pipe, condition or task).

```csharp
public Nullable<PipeStatus> Status { get; }
```

#### Property Value

[Nullable&lt;PipeStatus&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

### <a id="properties-targetsite"/>**TargetSite**

```csharp
public MethodBase TargetSite { get; }
```

#### Property Value

[MethodBase](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodbase)<br>

## Constructors

### <a id="constructors-.ctor"/>**PipeAndFilterException(PipeStatus, String)**

Create PipeAndFilter-Exception.

```csharp
public PipeAndFilterException(PipeStatus status, string message)
```

#### Parameters

`status` [PipeStatus](./pipefiltercore.pipestatus.md)<br>
The status of step (pipe, condition or task).

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The message that describes the error.

### <a id="constructors-.ctor"/>**PipeAndFilterException(PipeStatus, String, Exception)**

Create PipeAndFilter-Exception with innerException.

```csharp
public PipeAndFilterException(PipeStatus status, string message, Exception innerException)
```

#### Parameters

`status` [PipeStatus](./pipefiltercore.pipestatus.md)<br>
The status of step (pipe, condition or task).

`message` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The message that describes the error.

`innerException` [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
The exception that is the cause of the current exception, or a null reference.

## Methods

### <a id="methods-getobjectdata"/>**GetObjectData(SerializationInfo, StreamingContext)**

```csharp
public void GetObjectData(SerializationInfo info, StreamingContext context)
```

#### Parameters

`info` [SerializationInfo](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.serializationinfo)<br>

`context` [StreamingContext](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.streamingcontext)<br>


- - -
[**Back to List Api**](./apis.md)
