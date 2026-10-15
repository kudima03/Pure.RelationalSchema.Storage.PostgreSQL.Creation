# Changelog

All notable changes to `Pure.RelationalSchema.Storage.PostgreSQL.Creation` are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed

- Mark the package as AOT compatible (`IsAotCompatible` set to `true`); the AOT, trim and single-file analyzers now run on every target framework.

## [0.1.0-preview.0.1.0] - 2026-10-15

### Added

- Initial release: schema creation (`PostgreSqlCreatedSchema` and DDL statement builders) extracted from `Pure.RelationalSchema.Storage.PostgreSQL` 0.1.0-preview.8.1.2 without behavioural changes. See the [source package's CHANGELOG](https://github.com/kudima03/Pure.RelationalSchema.Storage.PostgreSQL/blob/main/CHANGELOG.md) for earlier history.
