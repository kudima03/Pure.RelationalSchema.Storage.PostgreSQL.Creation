using System.Data;
using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.Abstractions.String;
using Pure.Primitives.Bool;
using Pure.RelationalSchema.Abstractions.ForeignKey;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Abstractions.Table;

namespace Pure.RelationalSchema.Storage.PostgreSQL.Creation;

public sealed record PostgreSqlCreatedSchema : ISchema
{
    private readonly ISchema _schema;

    private readonly Lazy<IBool> _created;

    public PostgreSqlCreatedSchema(ISchema schema, IDbConnection connection)
    {
        _schema = schema;
        _created = new Lazy<IBool>(() =>
        {
            if (!new ForeignKeysWithinSchema(_schema).BoolValue)
            {
                throw new ArgumentException(
                    "Foreign keys must reference tables of the same schema."
                );
            }

            IDbCommand command = connection.CreateCommand();
            IString schemaCreationScript = new SchemaCreationStatement(_schema);
            command.CommandText = schemaCreationScript.TextValue;
            _ = command.ExecuteNonQuery();
            return new True();
        });
    }

    public IString Name =>
        _created.Value.BoolValue ? _schema.Name : throw new ArgumentException();

    public IEnumerable<ITable> Tables =>
        _created.Value.BoolValue ? _schema.Tables : throw new ArgumentException();

    public IEnumerable<IForeignKey> ForeignKeys =>
        _created.Value.BoolValue ? _schema.ForeignKeys : throw new ArgumentException();
}
