using Microsoft.Extensions.Options;
using NSE.Pagamentos.API.Models;
using NSE.Pagamentos.NerdsPag;

namespace NSE.Clientes.API.Facade
{
    public class PagamentoCartaoCreditoFacade : IPagamentoFacade
    {
        private readonly PagamentoConfig _pagmentoConfig;

        public PagamentoCartaoCreditoFacade(IOptions<PagamentoConfig> pagamentoConfig)
        {
            _pagmentoConfig = pagamentoConfig.Value;
        }

        public async Task<Transacao> AutorizarPagamento(Pagamento pagamento)
        {
            var nersdPagService = new NerdsPagService(_pagmentoConfig.DefaultApiKey, _pagmentoConfig.DefaultEncryptionKey);

            var cardHashGen = new CardHash(nersdPagService)
            {
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardCvv = pagamento.CartaoCredito.CVV,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento
            };

            var cardHash = cardHashGen.Generate();

            var transaction = new Transaction(nersdPagService)
            {
                CardHash = cardHash,
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardCvv = pagamento.CartaoCredito.CVV,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
                PaymentMethod = PaymentMethod.CreditCard,
                Amount = pagamento.Valor
            };

            return ParaTransacao(await transaction.AuthorizeCardTransaction());
        }

        public static Transacao ParaTransacao(Transaction transaction)
        {
            return new Transacao
            {
                Status = (StatusTransacao)transaction.Status,
                ValorTotal = transaction.Amount,
                BandeiraCartao = transaction.CardBrand,
                CodigoAutorizacao = transaction.AuthorizationCode,
                CustoTransacao = transaction.Cost,
                DataTransacao = transaction.TransactionDate,
                NSU = transaction.Nsu,
                TID = transaction.Tid
            };
        }
    }
}
