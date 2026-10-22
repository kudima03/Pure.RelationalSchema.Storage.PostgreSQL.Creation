using Pure.Primitives.Abstractions.Bool;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.ForeignKey;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Abstractions.Table;

namespace Pure.RelationalSchema.Storage.PostgreSQL.Creation;

internal sealed record ForeignKeyWithinSchema : IForeignKey
{
    private readonly IForeignKey _foreignKey;

    private readonly IBool _validState;

    public ForeignKeyWithinSchema(IForeignKey foreignKey, ISchema schema)
    {
        _foreignKey = foreignKey;
        _validState = new ForeignKeyWithinSchemaValidState(foreignKey, schema);
    }

    // Stryker disable once String
    public ITable ReferencingTable =>
        _validState.BoolValue
            ? _foreignKey.ReferencingTable
            : throw new ArgumentException(
                $"{nameof(ReferencingTable)} field is not valid"
            );

    // Stryker disable once String
    public IEnumerable<IColumn> ReferencingColumns =>
        _validState.BoolValue
            ? _foreignKey.ReferencingColumns
            : throw new ArgumentException(
                $"{nameof(ReferencingColumns)} field is not valid"
            );

    // Stryker disable once String
    public ITable ReferencedTable =>
        _validState.BoolValue
            ? _foreignKey.ReferencedTable
            : throw new ArgumentException(
                $"{nameof(ReferencedTable)} field is not valid"
            );

    // Stryker disable once String
    public IEnumerable<IColumn> ReferencedColumns =>
        _validState.BoolValue
            ? _foreignKey.ReferencedColumns
            : throw new ArgumentException(
                $"{nameof(ReferencedColumns)} field is not valid"
            );
}
