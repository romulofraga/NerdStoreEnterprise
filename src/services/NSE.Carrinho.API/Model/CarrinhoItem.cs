using FluentValidation;

namespace NSE.Carrinho.API.Model
{
    public class CarrinhoItem
    {
        private const int QUANTIDADE_MAX_ITEM = 5;
        public CarrinhoItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public string Imagem { get; set; }
        public decimal Valor { get; set; }
        public Guid CarrinhoId { get; set; }
        public CarrinhoCliente CarrinhoCliente { get; set; }

        internal void AssociarCarrinho(Guid carrinhoId)
        {
            CarrinhoId = carrinhoId;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * Valor;
        }

        internal void AdicionarUnidades(int unidades)
        {
            Quantidade += unidades;
        }

        internal bool IsValid()
        {
            return new ItemPedidoValidation().Validate(this).IsValid;
        }

        internal void AtualizarUnidades(int unidades)
        {
            Quantidade = unidades;
        }

        public class ItemPedidoValidation : AbstractValidator<CarrinhoItem>
        {
            public ItemPedidoValidation()
            {
                RuleFor(c => c.ProdutoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto inválido");
                RuleFor(c => c.Nome)
                    .NotEmpty()
                    .WithMessage("O Nome do produto não foi informado");
                RuleFor(c => c.Quantidade)
                    .GreaterThan(0)
                    .WithMessage("A quantidade mínima de item é 1");
                RuleFor(c => c.Quantidade)
                    .LessThan(QUANTIDADE_MAX_ITEM)
                    .WithMessage($"A quantidade máxima de item é {QUANTIDADE_MAX_ITEM}");
                RuleFor(c => c.Valor)
                    .GreaterThan(0)
                    .WithMessage("O valor do item precisa ser maior que 0");
            }
        }

    }
}
