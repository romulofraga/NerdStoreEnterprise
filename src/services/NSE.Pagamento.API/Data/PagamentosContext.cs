﻿#nullable disable
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NSE.Core.Data;
using NSE.Core.Messages;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Data;

public sealed class PagamentosContext : DbContext, IUnityOfWork
{
    public PagamentosContext(DbContextOptions<PagamentosContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SetDefaultIgnoreModels(modelBuilder);
        SetDefaultModelColumnsType(modelBuilder);
        SetDefaultBehaviorForeignKeys(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentosContext).Assembly);

        base.OnModelCreating(modelBuilder);

        #region Método locais para setar tipo default e relacionamento das FK

        void SetDefaultModelColumnsType(ModelBuilder modelBuilder)
        {
            // Todas colunas que são do tipo texto (string) terão como default o tamanho de varchar(100)
            var stringColumnsType = GetAllPropertiesByType(modelBuilder, typeof(string));

            foreach (var property in stringColumnsType) property.SetColumnType("varchar(100)");

            // Todas colunas que são do tipo decimal (valor numérico) terão como default o tamanho de decimal(18,2)
            var decimalColumnsType = GetAllPropertiesByType(modelBuilder, typeof(decimal));

            foreach (var property in decimalColumnsType) property.SetColumnType("decimal(18,2)");
        }

        IEnumerable<IMutableProperty> GetAllPropertiesByType(ModelBuilder modelBuilder, Type type)
        {
            return modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == type));
        }

        void SetDefaultBehaviorForeignKeys(ModelBuilder modelBuilder)
        {
            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in foreignKeys)
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        void SetDefaultIgnoreModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();
        }

        #endregion
    }
}