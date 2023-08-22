using NSE.Core.Data;

namespace NSE.Pedidos.Domain.Vouchers
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        void Atualizar(Voucher voucher);
        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}
