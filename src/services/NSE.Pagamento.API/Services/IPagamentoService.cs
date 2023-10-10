using NSE.Core.Messages.Integration;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Services
{
    public interface IPagamentoService
    {
        Task<ResponseMessage> AutorizarPagamento(Pagamento Pagamento);
    }
}
