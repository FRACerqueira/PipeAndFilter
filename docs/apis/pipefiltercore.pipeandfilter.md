# <img align="left" width="100" height="100" src="../images/icon.png">PipeAndFilter API:PipeAndFilter 

[![Build](https://github.com/FRACerqueira/PipeAndFilter/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PipeAndFilter/actions/workflows/build.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PipeAndFilter/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)
[![Downloads](https://img.shields.io/nuget/dt/PipeAndFilter)](https://www.nuget.org/packages/PipeAndFilter/)

[**Back to List Api**](./apis.md)

# PipeAndFilter

Namespace: PipeFilterCore

Represents PipeAndFilter Creator.

```csharp
public static class PipeAndFilter
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [PipeAndFilter](./pipefiltercore.pipeandfilter.md)

## Methods

### <a id="methods-new"/>**New&lt;T&gt;(Char)**

Create PipeAndFilter component.

```csharp
public static IPipeAndFilterStart<T> New<T>(char sepsameinstance)
```

#### Type Parameters

`T`<br>

#### Parameters

`sepsameinstance` [Char](https://docs.microsoft.com/en-us/dotnet/api/system.char)<br>
The separator char when it has the same instance. Default value is '#'
 <br>When the alias refers to the same instance and the alias is not informed, an alias is created with the name of the activated method, the char and a sequence starting from 1.

#### Returns

[IPipeAndFilterStart&lt;T&gt;](./pipefiltercore.ipipeandfilterstart-1.md)


- - -
[**Back to List Api**](./apis.md)
