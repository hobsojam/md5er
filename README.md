# md5er

A Windows native drag-and-drop file hash calculator.

Drop a file onto the window and it instantly displays the file's MD5, SHA-1, SHA-256, and SHA-512 checksums.

## Motivation

Tools like this already exist (e.g. HashCheck, 7-Zip's built-in hasher, CertUtil). This project isn't trying to replace them — it's a personal experiment in Windows native desktop development, exploring what it takes to build and ship a self-contained native Windows app from scratch.

## Tech Stack

- **Language:** C# 13
- **UI Framework:** WPF (Windows Presentation Foundation) on .NET 10
- **Hashing:** `System.Security.Cryptography` (built into .NET — no third-party libraries)
- **Testing:** xUnit
- **Editor:** VS Code with C# Dev Kit

## Output

A single self-contained `.exe` with no installer and no runtime dependencies, built with:

```
dotnet publish --self-contained -p:PublishSingleFile=true
```

## Usage

Run `md5er.exe` and drag any file onto the window. Hashes are displayed immediately and can be copied to the clipboard.

## Requirements (to build)

- [.NET 10 SDK](https://dotnet.microsoft.com)
- Windows (WPF is Windows-only)
