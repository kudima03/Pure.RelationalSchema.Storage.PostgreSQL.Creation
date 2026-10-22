using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.String.Operations;
using Pure.RelationalSchema.Abstractions.ForeignKey;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.HashCodes;

namespace Pure.RelationalSchema.Storage.PostgreSQL.Creation;

internal sealed record ForeignKeyWithinSchemaValidState : IBool
{
    private readonly Lazy<bool> _value;

    public ForeignKeyWithinSchemaValidState(IForeignKey foreignKey, ISchema schema)
    {
        _value = new Lazy<bool>(() =>
            new[] { foreignKey.ReferencingTable, foreignKey.ReferencedTable }.All(x =>
                schema.Tables.Any(c =>
                    new HexString(new TableHash(c)).TextValue
                    == new HexString(new TableHash(x)).TextValue
                )
            )
        );
    }

    public bool BoolValue => _value.Value;
}
