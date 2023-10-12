using Microsoft.Extensions.Options;
using NSE.Pagamentos.API.Models;
using NSE.Pagamentos.NerdsPag;

namespace NSE.Pagamentos.API.Facade;

public class PagamentoCartaoCreditoFacade : IPagamentoFacade
{
    private readonly PagamentoConfig _pagamentoConfig;

    public PagamentoCartaoCreditoFacade(IOptions<PagamentoConfig> pagamentoConfig)
    {
        _pagamentoConfig = pagamentoConfig.Value;
    }

    public async Task<Transacao> AutorizarPagamento(Pagamento pagamento)
    {
        var nersdPagService =
            new NerdsPagService(_pagamentoConfig.DefaultApiKey, _pagamentoConfig.DefaultEncryptionKey);

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

    public async Task<Transacao> CapturarPagamento(Transacao transacao)
    {
        var nerdsPagSvc = new NerdsPagService(_pagamentoConfig.DefaultApiKey,
            _pagamentoConfig.DefaultEncryptionKey);

        var transaction = ParaTransaction(transacao, nerdsPagSvc);

        return ParaTransacao(await transaction.CaptureCardTransaction());
    }

    public async Task<Transacao> CancelarAutorizacao(Transacao transacao)
    {
        var nerdsPagSvc = new NerdsPagService(_pagamentoConfig.DefaultApiKey,
            _pagamentoConfig.DefaultEncryptionKey);

        var transaction = ParaTransaction(transacao, nerdsPagSvc);

        return ParaTransacao(await transaction.CancelAuthorization());
    }


    private static Transacao ParaTransacao(Transaction transaction)
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

    private static Transaction ParaTransaction(Transacao transacao, NerdsPagService nerdsPagService)
    {
        return new Transaction(nerdsPagService)
        {
            Status = (TransactionStatus)transacao.Status,
            Amount = transacao.ValorTotal,
            CardBrand = transacao.BandeiraCartao,
            AuthorizationCode = transacao.CodigoAutorizacao,
            Cost = transacao.CustoTransacao,
            Nsu = transacao.NSU,
            Tid = transacao.TID
        };
    }
}