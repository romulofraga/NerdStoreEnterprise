using NetDevPack.Specification;

namespace NSE.Pedidos.Domain.Vouchers.Specs;

public class VoucherValidation : SpecValidator<Voucher>
{
    public VoucherValidation()
    {
        var dataSpec = new VoucherDataSpecification();
        var qdteSpec = new VoucherQuantidadeSpecification();
        var ativoSpec = new VoucherAtivoSpecification();

        Add("dataSpec", new Rule<Voucher>(dataSpec, "Este voucher esta expirado"));
        Add("qtdeSpec", new Rule<Voucher>(qdteSpec, "Este voucher ja foi utilizado"));
        Add("ativoSpec", new Rule<Voucher>(ativoSpec, "Este voucher nao esta mais ativo"));
    }
}