# Roslyn Analyzers - Code JoyRide

An example projects that includes Roslyn analyzers with code fix providers. Enjoy this template to learn from and modify analyzers for your own needs.

## Agenda

We will try to cover at least two of the following bullet points:

- Quick Introduction to Compilers and Roslyn
- Developing a Semantic Code Analyzer
- Providing a Code Fix Provider for the Analyzer

The workshop would be interactive and we do live coding.

## Prerequisites

- Some knowledge of programming 😁😜
- Laptop 💻
- [.NET Core](https://dotnet.microsoft.com/en-us/download) installed on your machine; see [all versions](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) for more information
- Have your preferred IDE installed;
    - [JetBrains Rider](https://www.jetbrains.com/rider/download/)
    - [VS Code](https://code.visualstudio.com/download) with [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp), [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) [Optional]
- Clone the [CodeJoyRide repository from GitHub](https://github.com/shahab-the-guy/code-joy-ride) if you want to implement with us step by step
- Last but not least, You 👩🏻‍💻👨🏻‍💻

## Content
### CodeJoyRide.Fx.Analyzer
A .NET Standard project with implementations of sample analyzers and code fix providers.
**You must build this project to see the results (warnings) in the IDE.**

- [MaybeSemanticAnalyzer.cs](SampleSemanticAnalyzer.cs): An analyzer that reports invalid usage of the `Maybe` type.
- [MaybeCodeFixProvider.cs](SampleCodeFixProvider.cs): A code fix that replaces a `throw` statement with `Maybe.None`. The fix is linked to [MaybeSemanticAnalyzer.cs](./CodeJoyRide.Fx.Analyzer/CodeJoyRide.Fx.Analyzer/MaybeSemanticAnalyzer.cs).

### CodeJoyRide.Api
A project that references the CodeJoyRide.Fx analyzers. Note the parameters of `ProjectReference` in [CodeJoyRide.Api.csproj](CodeJoyRide.Api/CodeJoyRide.Api.csproj), they make sure that the project is referenced as a set of analyzers.

### CodeJoyRide.Fx.Analyzer.Tests
Unit tests for the CodeJoyRide analyzers and code fix provider. The easiest way to develop language-related features is to start with unit tests.

## How To?
### How to debug?
- Use the [launchSettings.json](CodeJoyRide.Fx.Analyzer/CodeJoyRide.Fx.Analyzer/Properties/launchSettings.json) profile.
- Debug tests.

### How can I determine which syntax nodes I should expect?

* Consider using https://sharplab.io/ and set the Result to `Szntax Tree`
* On Visual Studio Code, consider installing the extension [CSharp Syntax Visualizer](https://marketplace.visualstudio.com/items?itemName=ypyl.syntax-visualizer-csharp)
* Consider installing the Roslyn syntax tree viewer plugin [Rossynt](https://plugins.jetbrains.com/plugin/16902-rossynt/).
  * Unfortunately, based on my experience this one is not working on Rider's latest versions.

### Learn more about wiring analyzers
The complete set of information is available at [roslyn github repo wiki](https://github.com/dotnet/roslyn/blob/main/docs/wiki/README.md).
