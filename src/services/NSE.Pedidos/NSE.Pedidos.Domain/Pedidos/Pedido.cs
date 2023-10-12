﻿using NSE.Core.DomainObjects;
using NSE.Pedidos.Domain.Vouchers;

namespace NSE.Pedidos.Domain.Pedidos;

public class Pedido : Entity, IAggregateRoot
{
    private readonly List<PedidoItem> _pedidoItems;

    public Pedido(Guid clienteId, decimal valorTotal, List<PedidoItem> pedidoItems, decimal desconto = 0,
        bool voucherUtilizado = false, Guid? voucherId = null)
    {
        ClienteId = clienteId;
        ValorTotal = valorTotal;
        _pedidoItems = pedidoItems;

        Desconto = desconto;
        VoucherUtilizado = voucherUtilizado;
        VoucherId = voucherId;
    }

    protected Pedido()
    {
    }

    public int Codigo { get; }
    public Guid ClienteId { get; private set; }
    public Guid? VoucherId { get; private set; }
    public bool VoucherUtilizado { get; private set; }
    public decimal Desconto { get; private set; }
    public decimal ValorTotal { get; private set; }
    public DateTime DataCadastro { get; }
    public PedidoStatus PedidoStatus { get; set; }

    public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

    public Endereco Endereco { get; private set; }

    //EF Relation
    public Voucher Voucher { get; private set; }

    public void AutorizarPedido()
    {
        PedidoStatus = PedidoStatus.Autorizado;
    }

    public void AtribuirVoucher(Voucher voucher)
    {
        VoucherUtilizado = true;
        VoucherId = voucher.Id;
        Voucher = voucher;
    }

    public void AtribuirEndereco(Endereco endereco)
    {
        Endereco = endereco;
    }

    public void CancelarPedido()
    {
        PedidoStatus = PedidoStatus.Cancelado;
    }

    public void FinalizarPedido()
    {
        PedidoStatus = PedidoStatus.Pago;
    }

    public void CalcularValorPedido()
    {
        ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
        CalcularValorTotalDesconto();
    }

    private void CalcularValorTotalDesconto()
    {
        if (!VoucherUtilizado) return;

        decimal desconto = 0;

        var valor = ValorTotal;

        if (Voucher.TipoDesconto == TipoDescontoVoucher.Porcentagem)
        {
            if (Voucher.Percentual.HasValue)
            {
                desconto = valor * Voucher.Percentual.Value / 100;
                valor -= desconto;
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }
    }
}