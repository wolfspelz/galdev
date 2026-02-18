---
paths:
  - "code/**/*"
  - "**/*.cs"
  - "**/*.cshtml"
  - "**/*.csproj"
  - "*.sln"
  - "Dockerfile"
  - ".github/**/*"
---

# Web Development

## Stack

- ASP.NET Core 8.0, Razor Pages + MVC
- C# with nullable enabled, implicit usings
- Markdig (Markdown), SixLabors.ImageSharp (images/WebP)
- Shared utility library: `code/galdevweb/n3q.Tools/`
- Localization: de-DE (default), en-US via `?lang=` query param or cookie

## Solution Structure

- `galdevweb.sln` at repo root
- `code/galdevweb/GaldevWeb/` — main web app
- `code/galdevweb/GaldevWeb.Test/` — xUnit tests
- `code/galdevweb/n3q.Tools/` — shared library
- `code/text2md/` — text-to-markdown converter
- `code/commit-dates/` — Python utility for commit dates

## Key Files

- `Program.cs` — app startup, middleware, localization config
- `GaldevConfig.cs` — paths, limits, cache settings
- `GaldevApp.cs` — main app logic, singleton service
- `TimelineEntry.cs` — YAML entry model
- `TimelineIndex.cs` — loads and indexes all entries from `data/data.yaml`

## Build & Test

```bash
dotnet build galdevweb.sln
dotnet test code/galdevweb/GaldevWeb.Test/
dotnet run --project code/galdevweb/GaldevWeb/
```

## Deployment

- Push to `deployment` branch triggers GitHub Actions CI/CD
- CI builds Docker image (`wolfspelz/galdevweb:latest`), pushes to DockerHub
- CD SSHs to prod server, pulls image, replaces container
- Use `/mypublish` skill to rebase deployment onto main and push
