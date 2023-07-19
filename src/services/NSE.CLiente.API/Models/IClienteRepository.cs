using NSE.Core.Data;

namespace NSE.Clientes.API.Models;

public interface IClienteRepository : IRepository<Cliente>
{
    void AdicionarCliente(Cliente cliente);
    Task<IEnumerable<Cliente>> ObterTodos();
    Task<Cliente> ObterPorCpf(string cpf);
}