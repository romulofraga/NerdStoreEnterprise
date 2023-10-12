﻿using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Core.Data;
using NSE.Core.Mediator;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Data;

public sealed class ClientesContext : DbContext, IUnityOfWork
{
    private readonly IMediatorHandler _mediatorHandler;

    public ClientesContext(DbContextOptions<ClientesContext> options, IMediatorHandler mediatorHandler)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
        _mediatorHandler = mediatorHandler;
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }

    public async Task<bool> Commit()
    {
        var sucesso = await SaveChangesAsync() > 0;

        if (sucesso) await _mediatorHandler.PublicarEventos(this);

        return sucesso;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();

        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                     e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);
    }
}