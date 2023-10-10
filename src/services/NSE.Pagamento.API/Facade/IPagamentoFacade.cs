using NSE.Pagamentos.API.Models;

namespace NSE.Clientes.API.Facade
{
    public interface IPagamentoFacade
    {
        Task<Transacao> AutorizarPagamento(Pagamento pagamento);
    }
}
