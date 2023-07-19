using FluentValidation;
using FluentValidation.Results;

namespace NSE.Carrinho.API.Model;

public class CarrinhoCliente
{
    public CarrinhoCliente()
    {
    }

    public CarrinhoCliente(Guid clienteId)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
    }

    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public List<CarrinhoItem> Itens { get; set; } = new();
    public ValidationResult ValidationResult { get; set; }

    internal bool CarrinhoItemExistente(CarrinhoItem item)
    {
        return Itens.Any(p => p.ProdutoId == item.ProdutoId);
    }

    internal CarrinhoItem ObterProdutoId(Guid produtoId)
    {
        return Itens.FirstOrDefault(p => p.ProdutoId == produtoId);
    }

    internal void CalcularValorCarrinho()
    {
        ValorTotal = Itens.Sum(p => p.CalcularValor());
    }

    internal void AdicionarItem(CarrinhoItem item)
    {
        if (!item.IsValid()) return;

        item.AssociarCarrinho(Id);

        if (CarrinhoItemExistente(item))
        {
            var itemExistente = ObterProdutoId(item.ProdutoId);
            itemExistente.AdicionarUnidades(item.Quantidade);

            item = itemExistente;
            Itens.Remove(itemExistente);
        }

        Itens.Add(item);

        CalcularValorCarrinho();
    }

    internal void AtualizarItem(CarrinhoItem item)
    {
        item.AssociarCarrinho(Id);

        var itemExistente = ObterProdutoId(item.ProdutoId);

        Itens.Remove(itemExistente);
        Itens.Add(item);

        CalcularValorCarrinho();
    }

    internal void AtualizarUnidades(CarrinhoItem item, int unidades)
    {
        item.AtualizarUnidades(unidades);
        AtualizarItem(item);
    }

    internal void RemoverItem(CarrinhoItem item)
    {
        var itemExistente = ObterProdutoId(item.ProdutoId);

        Itens.Remove(itemExistente);

        CalcularValorCarrinho();
    }

    internal bool IsValid()
    {
        var erros = Itens.SelectMany(i =>
            new CarrinhoItem.ItemCarrinhoValidation().Validate(i).Errors).ToList();

        erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);

        ValidationResult = new ValidationResult(erros);

        return ValidationResult.IsValid;
    }

    public class CarrinhoClienteValidation : AbstractValidator<CarrinhoCliente>
    {
        public CarrinhoClienteValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Cliente não reconhecido");
            RuleFor(c => c.Itens.Count)
                .GreaterThan(0)
                .WithMessage("O carrinho não possui items");
            RuleFor(c => c.ValorTotal)
                .GreaterThan(0)
                .WithMessage("O valor total do carrinho precisa ser maior que 0");
        }
    }
}