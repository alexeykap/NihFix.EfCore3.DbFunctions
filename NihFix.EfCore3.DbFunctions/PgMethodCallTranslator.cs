using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace NihFix.EfCore3.DbFunctions
{
    public class PgMethodCallTranslator:NpgsqlMethodCallTranslatorProvider 
    {
        /// <inheritdoc />
        public PgMethodCallTranslator(RelationalMethodCallTranslatorProviderDependencies dependencies, IRelationalTypeMappingSource typeMappingSource) : base(dependencies, typeMappingSource)
        {
            AddTranslators(new []{new PgAddDaysTranslator()});
        }
    }
}