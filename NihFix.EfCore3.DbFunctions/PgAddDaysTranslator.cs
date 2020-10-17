using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;

namespace NihFix.EfCore3.DbFunctions
{
    public class PgAddDaysTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _methodInfo =
            typeof(UsefulDbFunctions).GetMethod(nameof(UsefulDbFunctions.AddDays));

        /// <inheritdoc />
        public SqlExpression Translate(SqlExpression instance, MethodInfo method,
            IReadOnlyList<SqlExpression> arguments)
        {
            if (_methodInfo != method)
            {
                return null;
            }

            return new PgDateAddExpression(arguments);
        }
    }

    public class PgDateAddExpression : SqlExpression
    {
        private readonly Expression[] _arguments;

        public override bool CanReduce => false;

        /// <inheritdoc />
        public PgDateAddExpression(IReadOnlyCollection<Expression> arguments) : base(typeof(DateTime),new NpgsqlDateTypeMapping())
        {
            _arguments = arguments.ToArray();
        }
        
        

        /// <inheritdoc />
        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {        
            var intervalName = Expression.Constant("1 day", typeof(object));
            var interval = Expression.Convert(intervalName, typeof(TimeSpan));
            var multiply = Expression.Multiply(interval, _arguments[1]);
            var sum = Expression.Add(_arguments[0], multiply);
            return visitor.Visit(sum);
        }


        /// <inheritdoc />
        public override void Print(ExpressionPrinter expressionPrinter)
        {
        }
    }
}