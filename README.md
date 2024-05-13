# BpostSharp ✉️

This projects is a C# wrapper library to obtain Belgian city data from bpost (Belgian national mail service).


<details>
<summary>Table of Contents</summary>

- [BpostSharp ✉️](#bpostsharp-️)
  - [Prerequisites](#prerequisites)
  - [Building](#building)
  - [Installation](#installation)
  - [Getting started](#getting-started)
    - [Example code](#example-code)
  - [Code explanation](#code-explanation)
  - [Credits](#credits)

</details>

---

## Prerequisites
- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Building

Use [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) to build the project. 

## Installation

Get the NuGet packages from [nuget.org](https://www.nuget.org/) or search for `MichelMichels.BpostSharp` in the GUI package manager in Visual Studio.

You can also use the cli of the package manager with one of following commands:

```cli
Install-Package MichelMichels.BpostSharp.Excel
Install-Package MichelMichels.BpostSharp.Web
```

Above commands will also add a dependency to `MichelMichels.BpostSharp`. This nuget package contains the core code.

---

## Getting started

There are 2 versions of BpostSharp:

| Name                             | Nuget                                                                                                                                                  | Description           |
| -------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ | --------------------- |
| `MichelMichels.BpostSharp.Excel` | <a href="https://www.nuget.org/packages/MichelMichels.BpostSharp.Excel"><img src="https://img.shields.io/nuget/v/MichelMichels.BpostSharp.Excel"/></a> | Excel cache (offline) |
| `MichelMichels.BpostSharp.Web`   | <a href="https://www.nuget.org/packages/MichelMichels.BpostSharp.Web"><img src="https://img.shields.io/nuget/v/MichelMichels.BpostSharp.Web"/></a>     | Html cache (online)   |

These 2 versions have following dependency: 

| Name                       | Nuget                                                                                                                                      | Description                                                        |
| -------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------ |
| `MichelMichels.BpostSharp` | <a href="https://www.nuget.org/packages/MichelMichels.BpostSharp"><img src="https://img.shields.io/nuget/v/MichelMichels.BpostSharp"/></a> | This is the core package which contains all interfaces and models. |

To use the excel version, use any version of [the files at the official bpost website](https://www.bpost.be/nl/postcodevalidatie-tool).
To use the web version, grab one of the HTML versions on the same page.

### Example code

Creating a web instance:
```csharp
ICacheBuilder<CityData> webCacheBuilder = new WebCacheBuilder(BpostWebConstants.DutchEndpoint);
ICityDataService cityDataService = new BelgianCityDataService(webCacheBuilder);
```

Creating an excel instance:
```csharp
ICacheBuilder<CityData> webCacheBuilder = new ExcelCacheBuilder("excel-from-bpost.xls");
ICityDataService cityDataService = new BelgianCityDataService(webCacheBuilder);
```

Querying data:
```csharp
// Querying by postal code
CityData data = await cityDataService.GetByPostalCode("9000");

// Querying by city name
CityData data = await cityDataService.GetByCityName("Gent");
```

## Code explanation

The querying uses `StartsWith(...)` and ignores casing.

Data is saved into cache after executing `Build()` on the `ICacheBuilder` or after first query, whichever comes earlier. You can clear the cache with `Clear()`.

## Credits

Written by [Michel Michels](https://github.com/MichelMichels).
