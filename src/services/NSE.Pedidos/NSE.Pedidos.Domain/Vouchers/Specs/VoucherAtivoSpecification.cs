using System.Linq.Expressions;
using NetDevPack.Specification;

namespace NSE.Pedidos.Domain.Vouchers.Specs;

public class VoucherAtivoSpecification : Specification<Voucher>
{
    public override Expression<Func<Voucher, bool>> ToExpression()
    {
        return Voucher => Voucher.Ativo && !Voucher.Utilizado;
    }
}