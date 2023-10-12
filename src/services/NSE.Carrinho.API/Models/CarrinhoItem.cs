#nullable disable
using System.Text.Json.Serialization;
using FluentValidation.Results;
using NSE.Carrinho.API.Models.Validations;

namespace NSE.Carrinho.API.Models;

public class CarrinhoItem
{
    public CarrinhoItem()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
    public string Imagem { get; set; }
    public Guid CarrinhoClienteId { get; set; }

    public ValidationResult ValidationResult { get; set; }

    // EF Relation
    [JsonIgnore] public CarrinhoCliente CarrinhoCliente { get; set; }

    internal void AssociarCarrinho(Guid carrinhoId)
    {
        CarrinhoClienteId = carrinhoId;
    }

    internal decimal CalcularValorTotal()
    {
        return Valor * Quantidade;
    }

    internal void AdicionarUnidades(int unidades)
    {
        Quantidade += unidades;
    }

    internal void AtualizarUnidades(int unidades)
    {
        Quantidade = unidades;
    }

    internal bool EhValido()
    {
        return new ItemCarrinhoValidation().Validate(this).IsValid;
    }
}