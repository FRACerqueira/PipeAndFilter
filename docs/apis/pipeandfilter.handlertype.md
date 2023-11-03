# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:HandlerType 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# HandlerType

Namespace: PipeAndFilter

Represents a HandlerType

```csharp
public enum HandlerType
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [Enum](https://docs.microsoft.com/en-us/dotnet/api/system.enum) → [HandlerType](./pipeandfilter.handlertype.md)<br>
Implements [IComparable](https://docs.microsoft.com/en-us/dotnet/api/system.icomparable), [IFormattable](https://docs.microsoft.com/en-us/dotnet/api/system.iformattable), [IConvertible](https://docs.microsoft.com/en-us/dotnet/api/system.iconvertible)

## Fields

| Name | Value | Description |
| --- | --: | --- |
| Condition | 0 | Execution condition handler |
| ConditionGoto | 1 | Execution condition with goto handler |
| Pipe | 2 | Execution pipe handler |
| Task | 3 | Execution task handler |
| AggregateTask | 4 | Execution aggregate task handler |
| ConditionTask | 5 | Execution condition task handler |


- - -
[**Back to List Api**](./apis.md)
