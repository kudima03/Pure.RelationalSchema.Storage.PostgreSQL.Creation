using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.String.Operations;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.HashCodes;

namespace Pure.RelationalSchema.Storage.PostgreSQL.Creation;

internal sealed record ForeignKeysWithinSchema : IBool
{
    private readonly ISchema _schema;

    public ForeignKeysWithinSchema(ISchema schema)
    {
        _schema = schema;
    }

    public bool BoolValue =>
        _schema.ForeignKeys.All(x =>
            Contains(x.ReferencingTable) && Contains(x.ReferencedTable)
        );

    private bool Contains(ITable table)
    {
        return _schema.Tables.Any(x =>
            new HexString(new TableHash(x)).TextValue
            == new HexString(new TableHash(table)).TextValue
        );
    }
}
