using System.Data;
using Pure.Primitives.Bool;
using Pure.Primitives.Number;
using Pure.Primitives.Random.String;
using Pure.Primitives.String.Operations;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.ForeignKey;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.ColumnType;
using Pure.RelationalSchema.HashCodes;
using String = Pure.Primitives.String.String;

namespace Pure.RelationalSchema.Storage.PostgreSQL.Creation.Tests;

using Column = Column.Column;
using ForeignKey = ForeignKey.ForeignKey;
using Index = Index.Index;
using Schema = Schema.Schema;
using Table = Table.Table;

public sealed record PostgreSqlCreatedSchemaTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public PostgreSqlCreatedSchemaTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void CreateSchema()
    {
        IReadOnlyCollection<IColumn> columns1 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new StringColumnType()),
            new Column(new RandomString(new UShort(20)), new ULongColumnType()),
        ];

        IReadOnlyCollection<IColumn> columns2 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new TimeColumnType()),
            new Column(new RandomString(new UShort(20)), new IntColumnType()),
        ];

        ITable table1 = new Table(
            new String("Test"),
            columns1,
            [
                new Index(new True(), columns1.Take(1)),
                new Index(new True(), columns1.Skip(1).Take(1)),
                new Index(new False(), columns1.Skip(2).Take(2)),
            ]
        );

        ITable table2 = new Table(
            new String("Test"),
            columns2,
            [
                new Index(new True(), columns2.Take(1)),
                new Index(new True(), columns2.Skip(1).Take(1)),
                new Index(new False(), columns2.Skip(2).Take(2)),
            ]
        );

        IForeignKey foreignKey1 = new ForeignKey(
            table1,
            [table1.Columns.First()],
            table2,
            [table2.Columns.First()]
        );

        IForeignKey foreignKey2 = new ForeignKey(
            table1,
            [table1.Columns.Skip(1).First()],
            table2,
            [table2.Columns.Skip(1).First()]
        );

        ISchema schema = new Schema(
            new String("Test1"),
            [table1, table2],
            [foreignKey1, foreignKey2]
        );

        Assert.NotEmpty(new PostgreSqlCreatedSchema(schema, _fixture.Connection).Name);
    }

    [Fact]
    public void CreateSchemaWithNoColumns()
    {
        ITable table1 = new Table(new RandomString(new UShort(20)), [], []);

        ITable table2 = new Table(new RandomString(new UShort(20)), [], []);

        ISchema schema = new Schema(
            new RandomString(new UShort(20)),
            [table1, table2],
            []
        );

        Assert.NotEmpty(new PostgreSqlCreatedSchema(schema, _fixture.Connection).Name);
    }

    [Fact]
    public void CreateSchemaWithNoIndexes()
    {
        IReadOnlyCollection<IColumn> columns1 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new StringColumnType()),
            new Column(new RandomString(new UShort(20)), new ULongColumnType()),
        ];

        IReadOnlyCollection<IColumn> columns2 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new TimeColumnType()),
            new Column(new RandomString(new UShort(20)), new IntColumnType()),
        ];

        ITable table1 = new Table(new RandomString(new UShort(20)), columns1, []);

        ITable table2 = new Table(new RandomString(new UShort(20)), columns2, []);

        ISchema schema = new Schema(
            new RandomString(new UShort(20)),
            [table1, table2],
            []
        );

        Assert.NotEmpty(new PostgreSqlCreatedSchema(schema, _fixture.Connection).Name);
    }

    [Fact]
    public void CreateSchemaWithNoTables()
    {
        ISchema schema = new Schema(new RandomString(new UShort(20)), [], []);

        Assert.NotEmpty(new PostgreSqlCreatedSchema(schema, _fixture.Connection).Name);
    }

    [Fact]
    public void CreateSchemaWithNoForeignKeys()
    {
        IReadOnlyCollection<IColumn> columns1 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new StringColumnType()),
            new Column(new RandomString(new UShort(20)), new ULongColumnType()),
        ];

        IReadOnlyCollection<IColumn> columns2 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new TimeColumnType()),
            new Column(new RandomString(new UShort(20)), new IntColumnType()),
        ];

        ITable table1 = new Table(
            new RandomString(new UShort(20)),
            columns1,
            [
                new Index(new True(), columns1.Take(1)),
                new Index(new True(), columns1.Skip(1).Take(1)),
                new Index(new False(), columns1.Skip(2).Take(2)),
            ]
        );

        ITable table2 = new Table(
            new RandomString(new UShort(20)),
            columns2,
            [
                new Index(new True(), columns2.Take(1)),
                new Index(new True(), columns2.Skip(1).Take(1)),
                new Index(new False(), columns2.Skip(2).Take(2)),
            ]
        );

        ISchema schema = new Schema(
            new RandomString(new UShort(20)),
            [table1, table2],
            []
        );

        Assert.NotEmpty(new PostgreSqlCreatedSchema(schema, _fixture.Connection).Name);
    }

    [Fact]
    public void CreateSchemaWithCompositeForeignKeys()
    {
        IReadOnlyCollection<IColumn> columns1 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new IntColumnType()),
            new Column(new RandomString(new UShort(20)), new ULongColumnType()),
        ];

        IReadOnlyCollection<IColumn> columns2 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new IntColumnType()),
            new Column(new RandomString(new UShort(20)), new ULongColumnType()),
        ];

        ITable table1 = new Table(
            new RandomString(new UShort(20)),
            columns1,
            [
                new Index(new True(), columns1.Take(2)),
                new Index(new True(), columns1.TakeLast(2)),
            ]
        );

        ITable table2 = new Table(
            new RandomString(new UShort(20)),
            columns2,
            [
                new Index(new True(), columns2.Take(2)),
                new Index(new True(), columns2.TakeLast(2)),
            ]
        );

        IForeignKey foreignKey1 = new ForeignKey(
            table1,
            table1.Columns.Take(2),
            table2,
            table2.Columns.Take(2)
        );

        IForeignKey foreignKey2 = new ForeignKey(
            table1,
            table1.Columns.TakeLast(2),
            table2,
            table2.Columns.TakeLast(2)
        );

        ISchema schema = new Schema(
            new RandomString(new UShort(20)),
            [table1, table2],
            [foreignKey1, foreignKey2]
        );

        Assert.NotEmpty(new PostgreSqlCreatedSchema(schema, _fixture.Connection).Name);
    }

    [Fact]
    public void CreateSchemaOnExisting()
    {
        IReadOnlyCollection<IColumn> columns1 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new StringColumnType()),
            new Column(new RandomString(new UShort(20)), new ULongColumnType()),
        ];

        IReadOnlyCollection<IColumn> columns2 =
        [
            new Column(new RandomString(new UShort(20)), new DateColumnType()),
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
            new Column(new RandomString(new UShort(20)), new TimeColumnType()),
            new Column(new RandomString(new UShort(20)), new IntColumnType()),
        ];

        ITable table1 = new Table(
            new RandomString(new UShort(20)),
            columns1,
            [
                new Index(new True(), columns1.Take(1)),
                new Index(new True(), columns1.Skip(1).Take(1)),
                new Index(new False(), columns1.Skip(2).Take(2)),
            ]
        );

        ITable table2 = new Table(
            new RandomString(new UShort(20)),
            columns2,
            [
                new Index(new True(), columns2.Take(1)),
                new Index(new True(), columns2.Skip(1).Take(1)),
                new Index(new False(), columns2.Skip(2).Take(2)),
            ]
        );

        IForeignKey foreignKey1 = new ForeignKey(
            table1,
            [table1.Columns.First()],
            table2,
            [table2.Columns.First()]
        );

        IForeignKey foreignKey2 = new ForeignKey(
            table1,
            [table1.Columns.Skip(1).First()],
            table2,
            [table2.Columns.Skip(1).First()]
        );

        ISchema schema = new Schema(
            new RandomString(new UShort(20)),
            [table1, table2],
            [foreignKey1, foreignKey2]
        );

        Assert.NotEmpty(
            new PostgreSqlCreatedSchema(
                new PostgreSqlCreatedSchema(schema, _fixture.Connection),
                _fixture.Connection
            ).Name
        );
    }

    [Fact]
    public void ThrowsOnForeignKeyToTableOfAnotherSchema()
    {
        (ITable table, ITable otherSchemaTable) = TablePair();

        ISchema schema = new Schema(
            new RandomString(new UShort(20)),
            [table],
            [
                new ForeignKey(
                    table,
                    table.Columns.Take(1),
                    otherSchemaTable,
                    otherSchemaTable.Columns.Take(1)
                ),
            ]
        );

        _ = Assert.Throws<ArgumentException>(() =>
            new PostgreSqlCreatedSchema(schema, _fixture.Connection).Name
        );
        Assert.False(Exists(schema));
    }

    [Fact]
    public void ThrowsOnForeignKeyFromTableOfAnotherSchema()
    {
        (ITable table, ITable otherSchemaTable) = TablePair();

        ISchema schema = new Schema(
            new RandomString(new UShort(20)),
            [table],
            [
                new ForeignKey(
                    otherSchemaTable,
                    otherSchemaTable.Columns.Take(1),
                    table,
                    table.Columns.Take(1)
                ),
            ]
        );

        _ = Assert.Throws<ArgumentException>(() =>
            new PostgreSqlCreatedSchema(schema, _fixture.Connection).Name
        );
        Assert.False(Exists(schema));
    }

    private bool Exists(ISchema schema)
    {
        IDbCommand command = _fixture.Connection.CreateCommand();
        command.CommandText = $"""
            SELECT EXISTS (
                SELECT 1
                FROM information_schema.schemata
                WHERE schema_name = '{new HexString(
                    new SchemaHash(schema)
                ).TextValue[..63]}'
            )
            """;
        return (bool)command.ExecuteScalar()!;
    }

    private static (ITable, ITable) TablePair()
    {
        IReadOnlyCollection<IColumn> columns1 =
        [
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
        ];

        IReadOnlyCollection<IColumn> columns2 =
        [
            new Column(new RandomString(new UShort(20)), new LongColumnType()),
        ];

        return (
            new Table(
                new RandomString(new UShort(20)),
                columns1,
                [new Index(new True(), columns1)]
            ),
            new Table(
                new RandomString(new UShort(20)),
                columns2,
                [new Index(new True(), columns2)]
            )
        );
    }
}
