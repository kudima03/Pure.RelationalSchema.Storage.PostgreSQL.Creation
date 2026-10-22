# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet format --verify-no-changes             # check code style (CI enforces this)
dotnet format                                 # auto-fix code style
dotnet tool restore && dotnet csharpier check .   # CI enforces this too
dotnet csharpier format .                     # auto-fix csharpier formatting
dotnet test --no-build --verbosity normal     # run integration tests (requires Docker for Testcontainers)
dotnet pack --configuration Release -p:Version=<version> --output .
```

CI additionally passes `-p:AssemblyVersion` (pinned to the major) and `-p:FileVersion`; see `.github/workflows/publish-nuget.yml`.

## Architecture

This is the **schema creation (DDL)** package of the PostgreSQL storage for the Pure relational schema ecosystem. It was extracted from `Pure.RelationalSchema.Storage.PostgreSQL`; querying and insertion live in the sibling repositories `Pure.RelationalSchema.Storage.PostgreSQL.Querying` and `Pure.RelationalSchema.Storage.PostgreSQL.Insertion`. The only public type is `PostgreSqlCreatedSchema`.

**Key relationships:**

- `PostgreSqlCreatedSchema` wraps an `ISchema` and executes DDL (CREATE SCHEMA, CREATE TABLE, CREATE INDEX, ALTER TABLE … ADD CONSTRAINT) lazily via `SchemaCreationStatement` on first property access. All statements use `IF NOT EXISTS`.
- The schema, table and column names in PostgreSQL are derived from hash codes (via `Pure.RelationalSchema.HashCodes`), trimmed to 63 hex characters by `TrimmedHash`, rather than the model names, to avoid length and character constraints.
- All SQL statement builders (`SchemaCreationStatement`, `TableCreationStatement`, `TableCreationHeaderStatement`, `ColumnCreationStatement`, `PostgreSqlColumnTypeName`, `NotNullStatement`, `IndexCreationStatement`, `ForeignKeyCreationStatement`, `ForeignKeyStatement`, `AlterTableStatement`, `AddConstraintStatement`) are **internal** and compose SQL as `IString` values using `Pure.Primitives.String.Operations` combinators.

**Physical naming convention:** this package defines the physical naming convention (hash-derived, 63-character identifiers) that the Querying and Insertion packages depend on. `TrimmedHash` is deliberately duplicated in those packages. Changing `TrimmedHash` or the hash inputs is therefore a breaking change across all three packages.

**Testing:** Integration tests use `Testcontainers.PostgreSql` to spin up a real PostgreSQL instance. There are no unit tests with mocked databases. CI (`.github/workflows/build-and-test.yml`) runs mutation testing via `dotnet-stryker` at `Complete` level with a 59% break threshold and requires at least 83% line coverage (warning at 99%). CI also enforces `dotnet format` and `dotnet csharpier check .`.

**Multi-targeting:** net8.0, net9.0, net10.0. `IsAotCompatible` is `true` (AOT, trim and single-file analyzers run on every target).

**Package validation:** `EnablePackageValidation = true` with `PackageValidationBaselineVersion` currently `0.1.0-preview.0.3.0` (bumped after each release by the nuget-publish workflow). Breaking API changes against the baseline fail the build.

**Publishing:** triggered by pushing a semver tag matching `*.*.*`. The tag name becomes the `PackageVersion`. Packages are published to both GitHub Packages and NuGet.org.

## Code Style

Enforced via `.editorconfig`, `dotnet format --verify-no-changes` and `dotnet csharpier check .` in CI:

- No `var` — always use explicit types
- No expression-bodied methods or constructors — properties, indexers, accessors, and lambdas may be expression-bodied
- File-scoped namespace declarations
- Private fields prefixed with `_` (underscore camelCase)
- No non-private instance fields
- `using` directives outside the namespace, System directives sorted first
- Max line length: 90 characters
- Braces on new lines (`csharp_new_line_before_open_brace = all`)
- No implicit object creation (`new()` without type) when the type is apparent

## Commit Messages

Do not mention Claude or AI assistance in commit messages.
