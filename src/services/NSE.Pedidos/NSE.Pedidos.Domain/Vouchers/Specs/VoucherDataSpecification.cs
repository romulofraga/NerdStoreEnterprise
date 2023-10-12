using System.Linq.Expressions;
using NetDevPack.Specification;

namespace NSE.Pedidos.Domain.Vouchers.Specs;

public class VoucherDataSpecification : Specification<Voucher>
{
    public override Expression<Func<Voucher, bool>> ToExpression()
    {
        return Voucher => Voucher.DataValidade > DateTime.Now;
    }
}