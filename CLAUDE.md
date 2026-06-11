# md5er

## Project Overview

A Windows native drag-and-drop file hash calculator built with C# WPF on .NET 10. Drop a file onto the window; it displays MD5, SHA-1, SHA-256, and SHA-512 checksums. Personal experiment in Windows native desktop development — not intended to replace existing tools.

## Setup

1. Install .NET 10 SDK
2. `dotnet restore`
3. `dotnet build`

## Development Commands

```shell
dotnet build
dotnet run
dotnet test
dotnet publish --self-contained -p:PublishSingleFile=true -r win-x64
```

## Architecture

MVVM (Model-View-ViewModel) pattern:

- **Model** — hashing logic using `System.Security.Cryptography`, no UI dependencies
- **ViewModel** — exposes hash results and drag-drop state to the View via bindings
- **View** — XAML only; no logic in code-behind if it can go in the ViewModel

Test project (`md5er.Tests`) covers the Model and ViewModel layers with xUnit. The View is tested manually.

## Development Approach

- TDD: write tests first, then implementation
- Tests live in `md5er.Tests` (xUnit)
- Third-party packages allowed in the test project; **app project uses only built-in .NET packages**
- **Before starting any feature branch:** sync main first — `git checkout main && git pull`, then branch from there. Without this, branches diverge silently and accumulate merge conflicts.

## Security

- **NEVER** add secrets, API keys, credentials, or any sensitive data to git
- If a file might contain sensitive data, add it to `.gitignore` before touching it
- Only read dropped files — NEVER execute them

## Constraints

- No third-party NuGet packages in the app project
- Single window, no dialogs
- Target .NET 10, Windows only

## Style

- No comments unless the why is non-obvious
- No docstrings
- Prefer clear naming over explanation
