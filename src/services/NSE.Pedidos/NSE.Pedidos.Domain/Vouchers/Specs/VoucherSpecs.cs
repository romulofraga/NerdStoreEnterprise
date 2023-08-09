using NetDevPack.Specification;
using System.Linq.Expressions;

namespace NSE.Pedidos.Domain.Vouchers.Specs
{
    public class VoucherDataSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return Voucher => Voucher.DataValidade > DateTime.Now;
        }
    }

    public class VoucherQuantidadeSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return Voucher => Voucher.Quantidade > 0;
        }
    }

    public class VoucherAtivoSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return Voucher => Voucher.Ativo && !Voucher.Utilizado;
        }
    }
}
