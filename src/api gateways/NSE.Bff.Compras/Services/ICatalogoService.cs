using NSE.Bff.Compras.Models;

namespace NSE.Bff.Compras.Services;

public interface ICatalogoService
{
    Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> enumerable);
    Task<ItemProdutoDTO> ObterPorId(Guid id);
}