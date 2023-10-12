using NSE.Core.DomainObjects;
using NSE.Pedidos.Domain.Vouchers.Specs;

namespace NSE.Pedidos.Domain.Vouchers;

public class Voucher : Entity, IAggregateRoot
{
    public string Codigo { get; }
    public decimal? Percentual { get; }
    public decimal? ValorDesconto { get; }
    public int Quantidade { get; set; }
    public TipoDescontoVoucher TipoDesconto { get; }
    public DateTime DataCriacao { get; }
    public DateTime? DataUtilizacao { get; private set; }
    public DateTime DataValidade { get; }
    public bool Ativo { get; private set; }
    public bool Utilizado { get; private set; }

    public void DebitarQuantidade()
    {
        Quantidade -= 1;
        if (Quantidade >= 1) return;

        MarcarComoUtilizado();
    }

    public bool EstaValidoParaUtilizacao()
    {
        var spec = new VoucherAtivoSpecification()
            .And(new VoucherDataSpecification())
            .And(new VoucherQuantidadeSpecification());

        return spec.IsSatisfiedBy(this);
    }

    public void MarcarComoUtilizado()
    {
        Ativo = false;
        Utilizado = true;
        Quantidade = 0;
        DataUtilizacao = DateTime.Now;
    }
}