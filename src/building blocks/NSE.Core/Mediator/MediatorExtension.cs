using Microsoft.EntityFrameworkCore;
using NSE.Core.DomainObjects;

namespace NSE.Core.Mediator;

public static class MediatorExtension
{
    public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T contexto) where T : DbContext
    {
        var entities = contexto.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

        var domainEvents = entities
            .SelectMany(x => x.Entity.Notificacoes)
            .ToList();

        entities.ToList()
            .ForEach(entity => entity.Entity.LimparEventos());

        var tasks = domainEvents
            .Select(async domainEvent => { await mediator.PublicarEvento(domainEvent); });

        await Task.WhenAll(tasks);
    }
}