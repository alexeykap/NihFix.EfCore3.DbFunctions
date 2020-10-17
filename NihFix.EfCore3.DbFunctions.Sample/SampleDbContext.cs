using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace NihFix.EfCore3.DbFunctions.Sample
{
    public class SampleDbContext:DbContext
    {
     
        public virtual DbSet<SampleEntity> SampleEntities { get; set; }
        
        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost;User Id=postgres;Password=postgres;Port=5432;Database=UsefulFunctions;Persist Security Info=True;");
            optionsBuilder.ReplaceService<IMethodCallTranslatorProvider, PgMethodCallTranslator>();
            base.OnConfiguring(optionsBuilder);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDbFunction(typeof(UsefulDbFunctions).GetMethod(nameof(UsefulDbFunctions.AddDays)))
                .HasTranslation(a=>new PgDateAddExpression(a));
            base.OnModelCreating(modelBuilder);
        }
    }
}