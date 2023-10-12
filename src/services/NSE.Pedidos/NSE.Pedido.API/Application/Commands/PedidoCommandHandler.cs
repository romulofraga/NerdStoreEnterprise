using FluentValidation.Results;
using MediatR;
using NSE.Core.Messages;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Pedidos.API.Application.DTO;
using NSE.Pedidos.API.Application.Events;
using NSE.Pedidos.Domain.Pedidos;
using NSE.Pedidos.Domain.Vouchers;
using NSE.Pedidos.Domain.Vouchers.Specs;

namespace NSE.Pedidos.API.Application.Commands;

public class PedidoCommandHandler : CommandHandler, IRequestHandler<AdicionarPedidoCommand, ValidationResult>
{
    private readonly IMessageBus _bus;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IVoucherRepository _voucherRepository;

    public PedidoCommandHandler(IVoucherRepository voucherRepository, IPedidoRepository pedidoRepository,
        IMessageBus bus)
    {
        _voucherRepository = voucherRepository;
        _pedidoRepository = pedidoRepository;
        _bus = bus;
    }

    public async Task<ValidationResult> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
    {
        //Validar comando
        if (!message.IsValid()) return message.ValidationResult;

        //Mapear Pedido
        var pedido = MapearPedido(message);

        //Aplicar voucher
        if (!await AplicarVoucher(message, pedido)) return ValidationResult;

        //Validar Pedido
        if (!ValidarPedido(pedido)) return ValidationResult;

        //Processar pagamento
        if (!await ProcessarPagamento(pedido, message)) return ValidationResult;

        //Se Pagamento ok
        pedido.AutorizarPedido();

        //Adicionar Evento
        pedido.AdicionaEvento(new PedidoRealizadoEvent(pedido.Id, pedido.ClienteId));

        _pedidoRepository.Adicionar(pedido);

        return await PersistirDados(_pedidoRepository.UnityOfWork);
    }

    private async Task<bool> ProcessarPagamento(Pedido pedido, AdicionarPedidoCommand message)
    {
        var pedidoIniciado = new PedidoIniciadoIntegrationEvent
        {
            PedidoId = pedido.Id,
            ClienteId = pedido.ClienteId,
            Valor = pedido.ValorTotal,
            TipoPagamento = 1,
            NomeCartao = message.NomeCartao,
            NumeroCartao = message.NumeroCartao,
            MesAnoVencimento = message.ExpiracaoCartao,
            CVV = message.CVVCartao
        };

        var result = await _bus.RequestAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(pedidoIniciado);

        if (!result.ValidationResult.IsValid)
            foreach (var erro in result.ValidationResult.Errors)
                AdicionarErro(erro.ErrorMessage);

        return result.ValidationResult.IsValid;
    }

    private bool ValidarPedido(Pedido pedido)
    {
        var pedidoValorOriginal = pedido.ValorTotal;
        var pedidoDesconto = pedido.Desconto;

        pedido.CalcularValorPedido();

        if (pedido.ValorTotal != pedidoValorOriginal)
        {
            AdicionarErro("O valor do pedido nao confere com o calculo do pedido");
            return false;
        }

        if (pedido.Desconto != pedidoDesconto)
        {
            AdicionarErro("O valor do pedido nao confere com o calculo do pedido");
            return false;
        }

        return true;
    }

    private async Task<bool> AplicarVoucher(AdicionarPedidoCommand message, Pedido pedido)
    {
        if (!message.VoucherUtilizado) return true;

        var voucher = await _voucherRepository.ObterVoucherPorCodigo(message.VoucherCodigo);

        if (voucher == null)
        {
            AdicionarErro("O voucher informado nao existe");
            return false;
        }

        var voucherValidation = new VoucherValidation().Validate(voucher);
        if (!voucherValidation.IsValid)
        {
            voucherValidation.Errors.ToList().ForEach(error => AdicionarErro(error.ErrorMessage));
            return false;
        }

        pedido.AtribuirVoucher(voucher);
        voucher.DebitarQuantidade();

        _voucherRepository.Atualizar(voucher);

        return true;
    }

    private static Pedido MapearPedido(AdicionarPedidoCommand message)
    {
        var endereco = new Endereco
        {
            Logradouro = message.Endereco.Logradouro,
            Numero = message.Endereco.Numero,
            Bairro = message.Endereco.Bairro,
            Cep = message.Endereco.Cep,
            Cidade = message.Endereco.Cidade,
            Complemento = message.Endereco.Complemento,
            Estado = message.Endereco.Estado
        };

        var pedido = new Pedido(message.ClienteId, message.ValorTotal,
            message.PedidoItems.Select(PedidoItemDTO.ParaPedidoItem).ToList(), message.Desconto,
            message.VoucherUtilizado);

        pedido.AtribuirEndereco(endereco);
        return pedido;
    }
}