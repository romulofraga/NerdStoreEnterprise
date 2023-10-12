using NSE.Core.DomainObjects;

namespace NSE.Pedidos.Domain.Pedidos;

public class PedidoItem : Entity
{
    protected PedidoItem()
    {
    }

    public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario,
        string produtoImagem = null)
    {
        ProdutoId = produtoId;
        ProdutoNome = produtoNome;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
        ProdutoImagem = produtoImagem;
    }

    public Guid PedidoId { get; }
    public Guid ProdutoId { get; private set; }
    public string ProdutoNome { get; private set; }
    public int Quantidade { get; }
    public decimal ValorUnitario { get; }
    public string ProdutoImagem { get; set; }

    public Pedido Pedido { get; set; }

    internal decimal CalcularValor()
    {
        return Quantidade * ValorUnitario;
    }
}