# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:HandlerType 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# HandlerType

Namespace: PipeFilterCore

Represents a handler type.

```csharp
public enum HandlerType
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [Enum](https://docs.microsoft.com/en-us/dotnet/api/system.enum) → [HandlerType](./pipefiltercore.handlertype.md)<br>
Implements [IComparable](https://docs.microsoft.com/en-us/dotnet/api/system.icomparable), [IFormattable](https://docs.microsoft.com/en-us/dotnet/api/system.iformattable), [IConvertible](https://docs.microsoft.com/en-us/dotnet/api/system.iconvertible)

## Fields

| Name | Value | Description |
| --- | --: | --- |
| None | 0 | No Type, Runtime Initialize |
| Condition | 1 | Type condition. |
| ConditionGoto | 2 | Type condition with link to another pipe. |
| Pipe | 3 | Type pipe. |
| Task | 4 | Type task. |
| AggregateTask | 5 | Type aggregate task. |
| ConditionTask | 6 | Type condition task. |


- - -
[**Back to List Api**](./apis.md)
