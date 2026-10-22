# Pure.RelationalSchema.Storage.PostgreSQL.Creation

PostgreSQL schema creation (DDL) for the **Pure** relational schema ecosystem — `CREATE SCHEMA`, `CREATE TABLE`, `CREATE INDEX` and foreign keys generated from an `ISchema` and executed via `IDbConnection`.

[![.NET build & test](https://github.com/kudima03/Pure.RelationalSchema.Storage.PostgreSQL.Creation/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Storage.PostgreSQL.Creation/actions/workflows/build-and-test.yml)
[![publish NuGet](https://github.com/kudima03/Pure.RelationalSchema.Storage.PostgreSQL.Creation/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Storage.PostgreSQL.Creation/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.RelationalSchema.Storage.PostgreSQL.Creation)](https://www.nuget.org/packages/Pure.RelationalSchema.Storage.PostgreSQL.Creation)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

`Pure.RelationalSchema.Storage.PostgreSQL.Creation` turns the abstract relational model defined in `Pure.RelationalSchema` into a physical PostgreSQL schema. `PostgreSqlCreatedSchema` lazily generates and executes `CREATE SCHEMA`, `CREATE TABLE`, `CREATE INDEX` and `ALTER TABLE … ADD CONSTRAINT` statements derived from an `ISchema`.

This package covers DDL only. Reading and writing rows is handled by the related packages listed below.

## Public API

| Type | Kind | Description |
|------|------|-------------|
| `PostgreSqlCreatedSchema` | record | `ISchema` decorator that executes DDL lazily on first property access |

All SQL statement builders are internal.

## Usage

```csharp
using System.Data;
using Npgsql;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Storage.PostgreSQL.Creation;

// schema is your ISchema; connection is an open IDbConnection
IDbConnection connection = new NpgsqlConnection(connectionString);
connection.Open();

ISchema created = new PostgreSqlCreatedSchema(schema, connection);

// DDL runs lazily, the first time Name, Tables or ForeignKeys is accessed.
// Every statement uses IF NOT EXISTS, so running it again is safe.
IEnumerable<ITable> tables = created.Tables;
```

Foreign keys must stay within the schema: both the referencing and the referenced table must be among the schema's tables. A foreign key to or from a table of another schema is rejected with an `ArgumentException` on first access, before any SQL is executed.

## Naming convention

Schema, table and column identifiers in PostgreSQL are not the model names. Each one is the first 63 hexadecimal characters of the corresponding hash from `Pure.RelationalSchema.HashCodes` (`SchemaHash`, `TableHash`, `ColumnHash`), which avoids PostgreSQL's identifier length limit and character restrictions.

The Querying and Insertion packages rely on this convention to address the objects created here, so it is part of the contract between the packages.

## Dependencies

- [`Pure.RelationalSchema`](https://github.com/kudima03/Pure.RelationalSchema/tree/2.0.3) — core relational model abstractions: `ISchema`, `ITable`, `IForeignKey` and related types
- [`Pure.RelationalSchema.HashCodes`](https://github.com/kudima03/Pure.RelationalSchema.HashCodes/tree/3.3.0) — hash codes used to derive PostgreSQL object names
- [`Pure.Primitives.Abstractions`](https://github.com/kudima03/Pure.Primitives.Abstractions/tree/4.3.0) — base interfaces for primitive types (`IString`, `IBool`, etc.)
- [`Pure.Primitives`](https://github.com/kudima03/Pure.Primitives/tree/3.6.5) — concrete primitive type implementations
- [`Pure.Primitives.String.Operations`](https://github.com/kudima03/Pure.Primitives.String.Operations/tree/1.5.1) — string composition utilities used to build SQL statements
- [`Pure.Primitives.Choices`](https://github.com/kudima03/Pure.Primitives.Choices/tree/1.2.2) — choice types
- [`Pure.Primitives.Switches`](https://github.com/kudima03/Pure.Primitives.Switches/tree/1.1.3) — switch / pattern matching utilities

## Target Frameworks

- .NET 8
- .NET 9
- .NET 10

## Installation

```
dotnet add package Pure.RelationalSchema.Storage.PostgreSQL.Creation
```

## Related packages

- `Pure.RelationalSchema.Storage.PostgreSQL.Querying` — reading rows from tables created by this package
- `Pure.RelationalSchema.Storage.PostgreSQL.Insertion` — inserting rows into tables created by this package
